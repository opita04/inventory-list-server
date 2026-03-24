using System.ComponentModel.DataAnnotations;

namespace InventoryList.Models;

public class InventoryTemplate
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// JSON array of column mappings: [{ "Header": "Hostname", "Property": "Name" }, ...]
    /// </summary>
    public string MappingsJson { get; set; } = "[]";

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    public bool IsDeleted { get; set; } = false;

    public List<InventoryGroup> Groups { get; set; } = new();
}
