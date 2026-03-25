namespace InventoryList.Models;

public class SharedColumnMapping
{
    public string Header { get; set; } = string.Empty;
    public string Property { get; set; } = string.Empty;
    public string? ValidationRegex { get; set; }
    public string? SourceProperty { get; set; }
    public string? ExtractRegex { get; set; }
}
