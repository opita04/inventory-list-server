using System.ComponentModel.DataAnnotations;

namespace InventoryList.Models;

public class InventoryItem
{
    public int Id { get; set; }

    public int InventoryGroupId { get; set; }
    public InventoryGroup? InventoryGroup { get; set; }

    [MaxLength(200)]
    public string? Name { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }

    [MaxLength(50)]
    public string? IP { get; set; }

    [MaxLength(200)]
    public string? Domain { get; set; }

    [MaxLength(100)]
    public string? Onboarded { get; set; }

    [MaxLength(500)]
    public string? RecoveryProcess { get; set; }

    [MaxLength(50)]
    public string? RAM { get; set; }

    [MaxLength(50)]
    public string? Cores { get; set; }

    [MaxLength(100)]
    public string? Version { get; set; }

    [MaxLength(100)]
    public string? Edition { get; set; }

    [MaxLength(200)]
    public string? FullVersion { get; set; }

    [MaxLength(100)]
    public string? CUVersion { get; set; }

    [MaxLength(100)]
    public string? KBVersion { get; set; }

    [MaxLength(100)]
    public string? LastPatchedOn { get; set; }

    [MaxLength(100)]
    public string? Environment { get; set; }

    [MaxLength(100)]
    public string? OS { get; set; }

    [MaxLength(200)]
    public string? Product { get; set; }

    [MaxLength(200)]
    public string? Owner { get; set; }

    [MaxLength(1000)]
    public string? StatusNotes { get; set; }
}
