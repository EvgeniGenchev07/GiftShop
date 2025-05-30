using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer;
using Microsoft.EntityFrameworkCore;

namespace DataLayer
{
    public class UserContext : IDb<User, string>
    {
        private readonly GiftShopDbContext dbContext;
        public UserContext(GiftShopDbContext context)
        {
            dbContext = context;
        }

        public async Task Create(User item)
        {
            dbContext.Users.Add(item);
            await dbContext.SaveChangesAsync();
        }

        public async Task<User> Read(string key, bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<User> query = dbContext.Users;
            if (useNavigationalProperties) query = query.Include(q => q.Orders);
            if (isReadOnly) query = query.AsNoTrackingWithIdentityResolution();
            User user = await query.FirstOrDefaultAsync(x => x.Id == key);
            if (user is null) throw new KeyNotFoundException();
            return user;
        }

        public async Task<List<User>> ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<User> query = dbContext.Users;

            if (useNavigationalProperties)
            {
                query = query.Include(u => u.Orders);
            }

            if (isReadOnly)
            {
                query = query.AsNoTrackingWithIdentityResolution();
            }

            return query.ToList();
        }

        public async Task Update(User item, bool useNavigationalProperties = false)
        {
            User user = await Read(item.Id, useNavigationalProperties);
            dbContext.Entry(user).CurrentValues.SetValues(item);
            if (useNavigationalProperties)
            {
                
                List<Order> orders = new List<Order>();
                foreach (var ord in item.Orders)
                {
                    Order order = await dbContext.Orders.FirstOrDefaultAsync(x => x.Id == ord.Id);
                    if (order == null) orders.Add(ord);
                    else orders.Add(order);
                }
                user.Orders = item.Orders;
            }
            await dbContext.SaveChangesAsync();
        }

        public async Task Delete(string key)
        {
            User user = await Read(key);
            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync();
        }
    }
}
