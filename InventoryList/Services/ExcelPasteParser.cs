using InventoryList.Models;

namespace InventoryList.Services;

public static class ExcelPasteParser
{
    /// <summary>
    /// Expected column order — displayed as header row in the Bulk Add modal.
    /// </summary>
    public static readonly string[] ColumnHeaders =
    {
        "Name", "Notes", "IP", "Domain", "Onboarded", "RecoveryProcess",
        "RAM", "Cores", "Version", "Edition", "FullVersion", "CUVersion",
        "KBVersion", "LastPatchedOn", "Environment", "OS", "Product",
        "Owner", "StatusNotes"
    };

    /// <summary>
    /// Parses tab-delimited, newline-separated text (e.g. copied from Excel)
    /// into a list of InventoryItem objects.
    /// </summary>
    public static List<InventoryItem> Parse(string pastedText, int groupId)
    {
        var items = new List<InventoryItem>();

        if (string.IsNullOrWhiteSpace(pastedText))
            return items;

        var lines = pastedText.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in lines)
        {
            var trimmed = line.TrimEnd('\r');
            if (string.IsNullOrWhiteSpace(trimmed))
                continue;

            var cols = trimmed.Split('\t');

            // Skip if the row looks like a header row (matches our column headers)
            if (cols.Length > 0 && cols[0].Trim().Equals("Name", StringComparison.OrdinalIgnoreCase)
                && cols.Length >= 2 && cols[1].Trim().Equals("Notes", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            var item = new InventoryItem
            {
                InventoryGroupId = groupId,
                Name            = GetCol(cols, 0),
                Notes           = GetCol(cols, 1),
                IP              = GetCol(cols, 2),
                Domain          = GetCol(cols, 3),
                Onboarded       = GetCol(cols, 4),
                RecoveryProcess = GetCol(cols, 5),
                RAM             = GetCol(cols, 6),
                Cores           = GetCol(cols, 7),
                Version         = GetCol(cols, 8),
                Edition         = GetCol(cols, 9),
                FullVersion     = GetCol(cols, 10),
                CUVersion       = GetCol(cols, 11),
                KBVersion       = GetCol(cols, 12),
                LastPatchedOn   = GetCol(cols, 13),
                Environment     = GetCol(cols, 14),
                OS              = GetCol(cols, 15),
                Product         = GetCol(cols, 16),
                Owner           = GetCol(cols, 17),
                StatusNotes     = GetCol(cols, 18),
            };

            items.Add(item);
        }

        return items;
    }

    private static string? GetCol(string[] cols, int index)
    {
        if (index >= cols.Length) return null;
        var val = cols[index].Trim();
        return string.IsNullOrEmpty(val) ? null : val;
    }
}
