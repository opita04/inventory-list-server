using System.Text.Json;
using InventoryList.Models;

namespace InventoryList.Services;

public static class ExcelPasteParser
{
    public static readonly string[] DefaultColumnHeaders =
    {
        "Name", "Notes", "IP", "Domain", "Onboarded", "RecoveryProcess",
        "RAM", "Cores", "Version", "Edition", "FullVersion", "CUVersion",
        "KBVersion", "LastPatchedOn", "Environment", "OS", "Product",
        "Owner", "StatusNotes"
    };

    public static List<InventoryItem> Parse(string pastedText, int groupId, List<SharedColumnMapping>? activeMappings = null)
    {
        var items = new List<InventoryItem>();

        if (string.IsNullOrWhiteSpace(pastedText))
            return items;

        var mappings = activeMappings != null && activeMappings.Any()
            ? activeMappings
            : DefaultColumnHeaders.Select(h => new SharedColumnMapping { Header = h, Property = h }).ToList();
        var lines = pastedText.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in lines)
        {
            var trimmed = line.TrimEnd('\r');
            if (string.IsNullOrWhiteSpace(trimmed))
                continue;

            var cols = trimmed.Split('\t');

            // Skip header if it matches the first few columns of the mapping
            if (IsHeaderRow(cols, mappings))
                continue;

            var item = new InventoryItem { InventoryGroupId = groupId };

            for (int i = 0; i < cols.Length && i < mappings.Count; i++)
            {
                var mapping = mappings[i];
                var value = cols[i].Trim();
                if (string.IsNullOrEmpty(value)) continue;

                SetProperty(item, mapping.Property, value);
            }

            items.Add(item);
        }

        return items;
    }



    private static bool IsHeaderRow(string[] cols, List<SharedColumnMapping> mappings)
    {
        if (cols.Length == 0 || mappings.Count == 0) return false;
        // Check if the first column matches the first mapping header
        return cols[0].Trim().Equals(mappings[0].Header, StringComparison.OrdinalIgnoreCase);
    }

    private static void SetProperty(InventoryItem item, string propertyName, string value)
    {
        var prop = typeof(InventoryItem).GetProperty(propertyName);
        if (prop != null && prop.CanWrite && prop.PropertyType == typeof(string))
        {
            prop.SetValue(item, value);
        }
    }


}
