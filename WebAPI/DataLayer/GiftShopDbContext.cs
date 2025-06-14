using BusinessLayer;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataLayer;

public class GiftShopDbContext : IdentityDbContext<User>
{
    public GiftShopDbContext()
    {
    }

    public GiftShopDbContext(DbContextOptions optionsBuilder) : base(optionsBuilder)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<OrderedProduct> OrderedProducts { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Feedback> Feedbacks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured) optionsBuilder.UseSqlite("Data Source=gift_shop.db3");
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(u =>
        {
            u.HasMany(u => u.Orders)
                .WithOne(o => o.User);
            u.HasMany(u => u.Feedbacks)
                .WithOne(f => f.User);
        });
        modelBuilder.Entity<Product>().Property(p => p.ImagePath).IsRequired().HasMaxLength(70);
        modelBuilder.Entity<OrderedProduct>().HasOne(p => p.Product).WithMany();
        modelBuilder.Entity<Order>()
            .HasOne(u => u.User)
            .WithMany(u => u.Orders).IsRequired();
        base.OnModelCreating(modelBuilder);
    }
}
