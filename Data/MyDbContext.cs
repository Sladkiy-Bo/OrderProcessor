using Microsoft.EntityFrameworkCore; 
using System.Reflection.Metadata;
using OrderProcessor.Models;

namespace OrderProcessor.Data
{
    public class MyDbContext: DbContext
    {
       public MyDbContext(DbContextOptions<MyDbContext> options):
           base(options){}
        public MyDbContext(): base() {}

       public DbSet<Order> Orders {get; set;}

       protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.HasIndex(o => o.Id).IsUnique();
                entity.HasIndex(o => o.OrderNumber).IsUnique();

                entity.Property(o => o.Amount).HasColumnType("decimal(18,2)");
                entity.Property(o => o.Status).HasDefaultValue("Created");
                entity.HasIndex(o => o.Status);
            });
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=orders.db");
            }
        }
    }
}