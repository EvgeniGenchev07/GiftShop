using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer;
using Microsoft.EntityFrameworkCore;

namespace DataLayer
{
    public class OrderContext : IDb<Order, int>
    {
        private readonly GiftShopDbContext dbContext;
        public OrderContext(GiftShopDbContext dbContext)
        {
            this.dbContext = dbContext;

        }
        public async Task Create(Order item)
        {
            User userFromDb =await  dbContext.Users.FirstOrDefaultAsync(u => u.Id==item.User.Id);
            if (userFromDb != null) item.User = userFromDb;

            List<Product> products = new List<Product>(item.Products.Count);
            for (int i = 0; i < item.Products.Count; ++i)
            {
                Product productFromDb = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == item.Products[i].Id);
                if (productFromDb != null) products.Add(productFromDb);
                else products.Add(item.Products[i]);
            }
            item.Products = products; 

            dbContext.Orders.Add(item);
            await dbContext.SaveChangesAsync();

        }

       
        public async Task<Order> Read(int key, bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<Order> query = dbContext.Orders;
            if (useNavigationalProperties) query = query
            .Include(o => o.User)
            .Include(o => o.Products);

            if (isReadOnly) query = query.AsNoTrackingWithIdentityResolution();

            Order order = await query.FirstOrDefaultAsync(o => o.Id == key);

            if (order == null) throw new ArgumentException($"Order with Id{key} does not exist!");

            return order;
        }
        public async Task<List<Order>> ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<Order> query = dbContext.Orders;
            if (useNavigationalProperties) query = query
            .Include(o => o.User)
            .Include(o => o.Products);

            if (isReadOnly) query = query.AsNoTrackingWithIdentityResolution();

            return query.ToList();
        }

        public async Task Update(Order item, bool useNavigationalProperties = false)
        {
            Order orderFromDb = await Read(item.Id, useNavigationalProperties);

            dbContext.Entry<Order>(orderFromDb).CurrentValues.SetValues(item);

            if (useNavigationalProperties)
            {
             
                List<Product> products = new List<Product>(item.Products.Count);
                for (int i = 0; i < item.Products.Count; ++i)
                {
                   
                    Product productFromDb = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == item.Products[i].Id);
                    if (productFromDb != null) products.Add(productFromDb);
                    else products.Add(item.Products[i]);
                }
                orderFromDb.Products = products;
            }

            dbContext.SaveChanges();

        }

        public async Task Delete(int key)
        {

            Order orderFromDb =  await Read(key);
            dbContext.Orders.Remove(orderFromDb);
            await dbContext.SaveChangesAsync();
        }

       
    }
}
