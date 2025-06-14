using BusinessLayer;
using Microsoft.EntityFrameworkCore;

namespace DataLayer;

public class OrderContext : IDb<Order, int>
{
    private readonly GiftShopDbContext dbContext;

    public OrderContext(GiftShopDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task Create(Order item)
    {
        for (var i = 0; i < item.OrderedProducts.Count; ++i)
        {
            var productFromDb =
                await dbContext.Products.FirstOrDefaultAsync(p => p.Id == item.OrderedProducts[i].Product.Id);
            if (productFromDb != null)
            {
                item.OrderedProducts[i].Product = productFromDb;
                if (item.OrderedProducts[i].Quantity <= item.OrderedProducts[i].Product.Quantity)
                {
                    var product = item.OrderedProducts[i].Product;
                    product.Quantity -= item.OrderedProducts[i].Quantity;
                    if (product.Quantity <= 0)
                    {
                        product.Quantity = 0;
                        product.Status = ProductStatus.OutOfStock;
                    }
                }
                else
                {
                    throw new ArgumentException(
                        $"Not enough quantity of product {item.OrderedProducts[i].Product.Name}!");
                }
            }
        }

        dbContext.Orders.Add(item);
        await dbContext.SaveChangesAsync();
    }


    public async Task<Order> Read(int key, bool useNavigationalProperties = false, bool isReadOnly = false)
    {
        IQueryable<Order> query = dbContext.Orders;
        if (useNavigationalProperties)
            query = query
                .Include(o => o.User)
                .Include(o => o.OrderedProducts);

        if (isReadOnly) query = query.AsNoTrackingWithIdentityResolution();

        var order = await query.FirstOrDefaultAsync(o => o.Id == key);

        if (order == null) throw new ArgumentException($"Order with Id{key} does not exist!");

        return order;
    }

    public async Task<List<Order>> ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false)
    {
        IQueryable<Order> query = dbContext.Orders;
        if (useNavigationalProperties)
            query = query
                .Include(o => o.User)
                .Include(o => o.OrderedProducts)
                .ThenInclude(op => op.Product);

        if (isReadOnly) query = query.AsNoTrackingWithIdentityResolution();

        return query.ToList();
    }

    public async Task Update(Order item, bool useNavigationalProperties = false)
    {
        var orderFromDb = await Read(item.Id, useNavigationalProperties);

        dbContext.Entry(orderFromDb).CurrentValues.SetValues(item);

        if (useNavigationalProperties)
        {
            var OrderedProducts = new List<OrderedProduct>(item.OrderedProducts.Count);
            for (var i = 0; i < item.OrderedProducts.Count; ++i)
            {
                var productFromDb =
                    await dbContext.OrderedProducts.FirstOrDefaultAsync(p => p.Id == item.OrderedProducts[i].Id);
                if (productFromDb != null) OrderedProducts.Add(productFromDb);
                else OrderedProducts.Add(item.OrderedProducts[i]);
            }

            orderFromDb.OrderedProducts = OrderedProducts;
        }

        dbContext.SaveChanges();
    }

    public async Task Delete(int key)
    {
        var orderFromDb = await Read(key);
        dbContext.Orders.Remove(orderFromDb);
        await dbContext.SaveChangesAsync();
    }
}
