using Microsoft.EntityFrameworkCore; 
using System.Reflection.Metadata;
using OrderProcessor.Models;

namespace OrderProcessor.Data
{
    public class MyDbContext: DbContext
    {
       public MyDbContext(DbContextOptions<MyDbContext> options):
           base(options){}
       public DbSet<Order> Orders {set; get;}

       protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity(entity =>
            {
                entity.HasIndex(o => o.ID).IsUnique();
                entity.Property(o => o.Amount).HasColumnType("decimal(18,2)");
                entity.Property(o => o.Status).HasDefaultValue("Created");
                entity.HasIndex(o => o.Status);
            });
        }
    }
}