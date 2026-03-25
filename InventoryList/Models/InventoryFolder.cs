using System.ComponentModel.DataAnnotations;

namespace InventoryList.Models;

public class InventoryFolder
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public int SortOrder { get; set; } = 0;

    public bool IsDeleted { get; set; } = false;

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    public List<InventoryGroup> Groups { get; set; } = new();
}
