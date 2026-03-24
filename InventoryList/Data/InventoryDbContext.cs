using InventoryList.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryList.Data;

public class InventoryDbContext : DbContext
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options) { }

    public DbSet<InventoryGroup> InventoryGroups => Set<InventoryGroup>();
    public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InventoryGroup>(e =>
        {
            e.HasKey(g => g.Id);
            e.HasMany(g => g.Items)
             .WithOne(i => i.InventoryGroup)
             .HasForeignKey(i => i.InventoryGroupId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<InventoryItem>(e =>
        {
            e.HasKey(i => i.Id);
        });
    }
}
