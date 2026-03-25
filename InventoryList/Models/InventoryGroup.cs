using System.ComponentModel.DataAnnotations;

namespace InventoryList.Models;

public class InventoryGroup
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    public int? InventoryTemplateId { get; set; }
    public InventoryTemplate? InventoryTemplate { get; set; }

    public int? InventoryFolderId { get; set; }
    public InventoryFolder? InventoryFolder { get; set; }

    /// <summary>
    /// Stores per-group column widths and visibility: { "Name": 150, "IP": 100 }
    /// </summary>
    public string? ColumnSettingsJson { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    public bool IsDeleted { get; set; } = false;

    public List<InventoryItem> Items { get; set; } = new();
}
