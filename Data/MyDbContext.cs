using System.Reflection.Metadata;

namespace OrderProcessor.Data
{
    public class MyDbContext: DbContext
    {
       public MyDbContext(OnModelOptions<MyDbContext> options):
           base(options){}
       public DbSet<Order> orders {set; get;}

       protected override OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity(entity =>
            {
                entity.HasIndex(o => o.ID).IsUnique;
                entity.Property(o => o.Amount).HasColunmType("decimal(18,2)");
                entity.Property(o => o.Status).HasDefaultValue("Created");
                entity.HasIndex(o => o.Status);
            });
        }
    }
}