using System.ComponentModel.DataAnnotations;

namespace InventoryList.Models;

public class AuditLog
{
    public int Id { get; set; }
    
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    
    [MaxLength(100)]
    public string User { get; set; } = "Anonymous"; // Prep for AD
    
    [MaxLength(50)]
    public string Action { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string EntityType { get; set; } = string.Empty;
    
    public int EntityId { get; set; }
    
    [MaxLength(200)]
    public string EntityName { get; set; } = string.Empty;
    
    public string? Details { get; set; }
}
