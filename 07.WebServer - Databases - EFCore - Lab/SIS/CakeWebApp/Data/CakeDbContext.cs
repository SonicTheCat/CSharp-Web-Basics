namespace CakeWebApp.Data
{
    using CakeWebApp.Models;
    using Microsoft.EntityFrameworkCore;

    public class CakeDbContext : DbContext
    {
        public CakeDbContext()
        {
        }

        public CakeDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<OrderProduct> OrderProducts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=.;Database=CakeWebApp;Trusted_Connection=True");
            }
        }
    }
}