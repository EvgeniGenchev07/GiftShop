using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

try
{
    var options = new IdentityOptions();
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 5;

    var builder = new DbContextOptionsBuilder<GiftShopDbContext>();
    builder.UseSqlite("DataSource=../../../../DataLayer/gift_shop.db3");

    var dbContext = new GiftShopDbContext(builder.Options);
    var userManager = new UserManager<User>(
        new UserStore<User>(dbContext), Options.Create(options),
        new PasswordHasher<User>(), new List<IUserValidator<User>> { new UserValidator<User>() },
        new List<IPasswordValidator<User>>(), new UpperInvariantLookupNormalizer(),
        new IdentityErrorDescriber(), new ServiceCollection().BuildServiceProvider(),
        new Logger<UserManager<User>>(new LoggerFactory())
    );

    var identityContext = new IdentityContext(dbContext, userManager);

    dbContext.Roles.Add(new IdentityRole("Administrator") { NormalizedName = "ADMINISTRATOR" });
    dbContext.Roles.Add(new IdentityRole("User") { NormalizedName = "USER" });
    await dbContext.SaveChangesAsync();
    Console.WriteLine("Roles added succssfully!");
    var user = new User("admin", "admincho@abv.bg");
    await identityContext.CreateUserAsync("admin", "admin", "adminov@abv.bg", Role.Administrator);

    Console.WriteLine("Admin account added successfully!");
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
