using Microsoft.EntityFrameworkCore;
using SportStore.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SportStore.Infrastructure.Data;

public class SportStoreDbContext : IdentityDbContext<ApplicationUser>
{
    public SportStoreDbContext(DbContextOptions<SportStoreDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderLine> OrderLines => Set<OrderLine>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Product entity
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Price)
                .HasColumnType("decimal(18,2)");
            entity.Property(e => e.ImageURL).HasMaxLength(200);
            
            entity.HasOne(e => e.Category)
                  .WithMany(c => c.Products)
                  .HasForeignKey(e => e.CategoryId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Category entity
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
        });

        // Configure Order entity
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.DeliveryName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.DeliveryAddress).IsRequired().HasMaxLength(200);
            entity.Property(e => e.DeliveryCity).IsRequired().HasMaxLength(100);
            entity.Property(e => e.DeliveryPostalCode).IsRequired().HasMaxLength(20);
            entity.Property(e => e.OrderDate).IsRequired();
        });

        // Configure OrderLine entity
        modelBuilder.Entity<OrderLine>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ProductName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.PriceAtOrder).HasColumnType("decimal(18,2)");
            entity.HasOne(e => e.Order)
                  .WithMany(o => o.OrderLines)
                  .HasForeignKey(e => e.OrderId);
        });

        // Seed Categories
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Running" },
            new Category { Id = 2, Name = "Football" },
            new Category { Id = 3, Name = "Tennis" },
            new Category { Id = 4, Name = "Swimming" },
            new Category { Id = 5, Name = "Basketball" }
        );
    }
} 