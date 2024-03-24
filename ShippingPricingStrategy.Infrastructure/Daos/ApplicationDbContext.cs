using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ShippingPricingStrategy.Domain.Models.Entities;

namespace ShippingPricingStrategy.Infrastructure.Daos;

public class ApplicationDbContext : DbContext
{
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions)
        : base(dbContextOptions)
    {
    }
    
    public virtual DbSet<Cart> Carts { get; set; }
    
    public virtual DbSet<Customer> Customers { get; set; }
    
    public virtual DbSet<Service> Services { get; set; }
    
    public virtual DbSet<Price> Prices { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer();
        }
    }
}