using BusinessLayer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DataLayer;

public class IdentityContext
{
    private readonly GiftShopDbContext _context;
    private readonly UserManager<User> _userManager;

    public IdentityContext(GiftShopDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }


    public async Task CreateUserAsync(string name, string password, string email, Role role)
    {
        try
        {
            var user = new User(email, name);
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded) throw new ArgumentException(result.Errors.First().Description);

            if (role == Role.Administrator)
                await _userManager.AddToRoleAsync(user, Role.Administrator.ToString());
            else if (role == Role.User) await _userManager.AddToRoleAsync(user, Role.User.ToString());
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<User> LogInUserAsync(string email, string password)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) return null;

            var result = await _userManager.PasswordValidators[1].ValidateAsync(_userManager, user, password);

            if (result.Succeeded) return await _context.Users.FindAsync(user.Id);

            return null;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<Order> GetUserLastOrder(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        return user == null ? null : user.Orders.Last();
    }

    public async Task<User> ReadUserAsync(string key)
    {
        var query = _context.Users
            .Include(o => o.Orders)
            .ThenInclude(o => o.OrderedProducts)
            .ThenInclude(op => op.Product)
            .Include(o => o.Feedbacks);
        var user = await query.FirstOrDefaultAsync(u => u.Id == key);
        return user;
    }

    public async Task<IEnumerable<User>> ReadAllUsersAsync()
    {
        try
        {
            return await _context.Users.ToListAsync();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /* public async Task UpdateUserAsync(string id, string username, string name, int age)
     {
         try
         {
             if (!string.IsNullOrEmpty(username))
             {
                 User user = await context.Users.FindAsync(id);
                 context.Entry(user).CurrentValues.SetValues(user);
                 await userManager.UpdateAsync(user);
             }
         }
         catch (Exception ex)
         {
             throw ex;
         }
     }
*/
    public async Task DeleteUserByNameAsync(string name)
    {
        try
        {
            var user = await FindUserByNEmailAsync(name);

            if (user == null) throw new InvalidOperationException("User not found for deletion!");

            await _userManager.DeleteAsync(user);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<User> FindUserByNEmailAsync(string name)
    {
        try
        {
            // Identity return Null if there is no user!
            return await _userManager.FindByEmailAsync(name);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
