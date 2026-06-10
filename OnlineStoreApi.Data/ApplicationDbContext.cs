using Microsoft.EntityFrameworkCore;
using OnlineStoreApi.Data.Models;

namespace OnlineStoreApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Product -> ProductCategory relationship
            modelBuilder.Entity<Product>()
                .HasOne(p => p.ProductCategory)
                .WithMany(pc => pc.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Order -> Customer relationship
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // OrderDetail -> Order relationship
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // OrderDetail -> Product relationship
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(od => od.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes for performance
            modelBuilder.Entity<Product>()
                .HasIndex(p => p.CategoryId);

            modelBuilder.Entity<Order>()
                .HasIndex(o => o.CustomerId);

            modelBuilder.Entity<OrderDetail>()
                .HasIndex(od => od.OrderId);

            modelBuilder.Entity<OrderDetail>()
                .HasIndex(od => od.ProductId);
        }
    }
}
