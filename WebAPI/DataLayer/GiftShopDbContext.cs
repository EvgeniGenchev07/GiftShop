using BusinessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DataLayer
{
    public class GiftShopDbContext:IdentityDbContext<User>
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderedProduct> OrderedProducts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }


        public GiftShopDbContext() : base() { }
        public GiftShopDbContext(DbContextOptions optionsBuilder) : base(optionsBuilder) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured) optionsBuilder.UseSqlite("Data Source=gift_shop.db3");
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(u =>
            {
                u.HasMany(u => u.Orders)
                .WithOne(o => o.User);
            });
            modelBuilder.Entity<Product>().Property(p=>p.ImagePath).IsRequired().HasMaxLength(70);
            modelBuilder.Entity<OrderedProduct>().HasOne(p => p.Product).WithMany();
            modelBuilder.Entity<Order>()
                .HasOne(u => u.User)
                .WithMany(u => u.Orders).IsRequired();
            base.OnModelCreating(modelBuilder);
        }
    }
}
