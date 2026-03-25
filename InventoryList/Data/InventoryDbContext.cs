using InventoryList.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryList.Data;

public class InventoryDbContext : DbContext
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options) { }

    public DbSet<InventoryGroup> InventoryGroups => Set<InventoryGroup>();
    public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();
    public DbSet<InventoryTemplate> InventoryTemplates => Set<InventoryTemplate>();
    public DbSet<InventoryFolder> InventoryFolders => Set<InventoryFolder>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    public override int SaveChanges()
    {
        ProcessSoftDeletesAndAudits();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        ProcessSoftDeletesAndAudits();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void ProcessSoftDeletesAndAudits()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)
            .ToList();

        var audits = new List<AuditLog>();
        var addedItemsCount = 0;

        foreach (var entry in entries)
        {
            if (entry.Entity is AuditLog) continue;

            // Check for bulk inserts of InventoryItem
            if (entry.State == EntityState.Added && entry.Entity is InventoryItem)
            {
                addedItemsCount++;
                continue; // Skip individual audit logic for items, handled below
            }

            string action = entry.State.ToString();
            var nameProp = entry.Entity.GetType().GetProperty("Name");
            string nameVal = nameProp?.GetValue(entry.Entity)?.ToString() ?? "Unknown";

            var idProp = entry.Entity.GetType().GetProperty("Id");
            int entityId = idProp != null && entry.State != EntityState.Added ? (int)idProp.GetValue(entry.Entity)! : 0;

            if (entry.State == EntityState.Deleted)
            {
                if (entry.Entity is InventoryGroup g) { g.IsDeleted = true; entry.State = EntityState.Modified; action = "Deleted"; }
                else if (entry.Entity is InventoryTemplate t) { t.IsDeleted = true; entry.State = EntityState.Modified; action = "Deleted"; }
                else if (entry.Entity is InventoryFolder f) { f.IsDeleted = true; entry.State = EntityState.Modified; action = "Deleted"; }
            }
            else if (entry.State == EntityState.Modified)
            {
                var isDelProp = entry.Metadata.FindProperty("IsDeleted");
                if (isDelProp != null)
                {
                    var propEntry = entry.Property("IsDeleted");
                    if (propEntry.IsModified)
                    {
                        bool wasDeleted = (bool)(propEntry.OriginalValue ?? false);
                        bool isNowDeleted = (bool)(propEntry.CurrentValue ?? false);
                        if (wasDeleted && !isNowDeleted) action = "Restored";
                        else if (!wasDeleted && isNowDeleted) action = "Deleted";
                        else action = "Updated";
                    }
                    else action = "Updated";
                }
                else action = "Updated";
            }

            audits.Add(new AuditLog
            {
                Timestamp = DateTime.UtcNow,
                User = "Anonymous (Pending AD)",
                Action = action,
                EntityType = entry.Entity.GetType().Name,
                EntityName = nameVal,
                EntityId = entityId
            });
        }

        if (addedItemsCount > 0)
        {
            if (addedItemsCount == 1)
            {
                var singleItem = entries.First(e => e.State == EntityState.Added && e.Entity is InventoryItem).Entity as InventoryItem;
                audits.Add(new AuditLog
                {
                    Timestamp = DateTime.UtcNow,
                    User = "Anonymous (Pending AD)",
                    Action = "Added",
                    EntityType = "InventoryItem",
                    EntityName = singleItem?.Name ?? "Unknown"
                });
            }
            else 
            {
                audits.Add(new AuditLog
                {
                    Timestamp = DateTime.UtcNow,
                    User = "Anonymous (Pending AD)",
                    Action = "Added",
                    EntityType = "InventoryItem",
                    EntityName = $"{addedItemsCount} rows",
                    Details = $"Bulk added {addedItemsCount} rows."
                });
            }
        }

        if (audits.Count > 0)
        {
            AuditLogs.AddRange(audits);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InventoryTemplate>(e =>
        {
            e.HasKey(t => t.Id);
            e.HasMany(t => t.Groups)
             .WithOne(g => g.InventoryTemplate)
             .HasForeignKey(g => g.InventoryTemplateId)
             .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<InventoryFolder>(e =>
        {
            e.HasKey(f => f.Id);
            e.HasMany(f => f.Groups)
             .WithOne(g => g.InventoryFolder)
             .HasForeignKey(g => g.InventoryFolderId)
             .OnDelete(DeleteBehavior.SetNull);
        });

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
