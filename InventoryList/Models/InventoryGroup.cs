using System.ComponentModel.DataAnnotations;

namespace InventoryList.Models;

public class InventoryGroup
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    public List<InventoryItem> Items { get; set; } = new();
}
