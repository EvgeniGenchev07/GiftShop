using BusinessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class FeedbackContext : IDb<Feedback, int>
    {
        private readonly GiftShopDbContext dbContext;
        public FeedbackContext(GiftShopDbContext context)
        {
            dbContext = context;
        }

        public async Task Create(Feedback item)
        {
            dbContext.Feedbacks.Add(item);
            await dbContext.SaveChangesAsync();
        }

        public async Task<Feedback> Read(int key, bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<Feedback> query = dbContext.Feedbacks;
            if (isReadOnly) query = query.AsNoTrackingWithIdentityResolution();
            Feedback feedback = await query.FirstOrDefaultAsync(x => x.Id == key);
            if (feedback is null) throw new KeyNotFoundException();
            return feedback;
        }

        public async Task<List<Feedback>> ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<Feedback> query = dbContext.Feedbacks;
            if (isReadOnly)
            {
                query = query.AsNoTrackingWithIdentityResolution();
            }
            return query.ToList();
        }

        public async Task Update(Feedback item, bool useNavigationalProperties = false)
        {
            Feedback feedback = await Read(item.Id, useNavigationalProperties);
            dbContext.Entry(feedback).CurrentValues.SetValues(item);
            await dbContext.SaveChangesAsync();
        }

        public async Task Delete(int key)
        {
            Feedback feedback = await Read(key);
            dbContext.Feedbacks.Remove(feedback);
            await dbContext.SaveChangesAsync();
        }
    }
}
