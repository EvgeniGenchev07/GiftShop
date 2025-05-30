using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer;
using Microsoft.EntityFrameworkCore;

namespace DataLayer
{
    public class ProductContext : IDb<Product, int>
    {
        private readonly GiftShopDbContext dbContext;
        public ProductContext(GiftShopDbContext context)
        {
            dbContext = context;
        }
        public  async Task Create(Product item)
        {
            dbContext.Products.Add(item);
            await dbContext.SaveChangesAsync();
        }

        
        public  async Task<Product> Read(int key, bool useNavigationalProperties = false, bool isReadOnly = false)
        {
          
            IQueryable<Product> query = dbContext.Products;
            if (isReadOnly) query = query.AsNoTrackingWithIdentityResolution();
            Product product = await query.FirstOrDefaultAsync(p => p.Id == key);
            if (product is null) throw new ArgumentException($"Product with Id {key} does not exist!");
            return product;
        }

        public async Task<List<Product>> ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<Product> query = dbContext.Products;
            if (isReadOnly) query = query.AsNoTrackingWithIdentityResolution();
            return query.ToList();

        }

        public  async Task Update(Product item, bool useNavigationalProperties = false)
        {
             
            Product productFromDb =  await Read(item.Id, useNavigationalProperties);
            dbContext.Entry<Product>(productFromDb).CurrentValues.SetValues(item);
            
            await dbContext.SaveChangesAsync();
        }
        public async Task Delete(int key)
        {
            Product product = await Read(key);
            dbContext.Products.Remove(product);
            await dbContext.SaveChangesAsync();
        }
    }
}
