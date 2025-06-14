using BusinessLayer;
using Microsoft.EntityFrameworkCore;

namespace DataLayer;

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
        if (useNavigationalProperties) query = query.Include(x => x.User);
        var feedback = await query.FirstOrDefaultAsync(x => x.Id == key);
        if (feedback is null) throw new KeyNotFoundException();
        return feedback;
    }

    public async Task<List<Feedback>> ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false)
    {
        IQueryable<Feedback> query = dbContext.Feedbacks;
        if (isReadOnly) query = query.AsNoTrackingWithIdentityResolution();
        if (useNavigationalProperties) query = query.Include(x => x.User);

        return query.ToList();
    }

    public async Task Update(Feedback item, bool useNavigationalProperties = false)
    {
        var feedback = await Read(item.Id, useNavigationalProperties);
        dbContext.Entry(feedback).CurrentValues.SetValues(item);
        await dbContext.SaveChangesAsync();
    }

    public async Task Delete(int key)
    {
        var feedback = await Read(key);
        dbContext.Feedbacks.Remove(feedback);
        await dbContext.SaveChangesAsync();
    }
}
