using ClosedXML.Excel;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Reflection;
using InventoryList.Models;

namespace InventoryList.Services;

public record ImportSheet(string Name, string[] Headers, List<string[]> Rows);

public static class FileImportService
{
    private static readonly string[] AllowedExtensions = { ".xlsx", ".csv" };

    private static readonly List<string> ItemProperties = typeof(InventoryItem)
        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
        .Where(p => p.PropertyType == typeof(string) && p.Name != "InventoryGroup")
        .Select(p => p.Name)
        .ToList();

    public static bool IsAllowedExtension(string fileName)
    {
        var ext = Path.GetExtension(fileName).ToLowerInvariant();
        return AllowedExtensions.Contains(ext);
    }

    public static List<ImportSheet> ParseExcel(Stream stream)
    {
        var sheets = new List<ImportSheet>();
        using var workbook = new XLWorkbook(stream);

        foreach (var ws in workbook.Worksheets)
        {
            if (ws.IsEmpty()) continue;

            var usedRange = ws.RangeUsed();
            if (usedRange == null) continue;

            var firstRow = usedRange.FirstRow();
            var headers = firstRow.CellsUsed()
                .Select(c => c.GetString().Trim())
                .ToArray();

            if (headers.Length == 0) continue;

            var rows = new List<string[]>();
            var colCount = headers.Length;

            foreach (var row in usedRange.RowsUsed().Skip(1))
            {
                var values = new string[colCount];
                for (int i = 0; i < colCount; i++)
                {
                    values[i] = row.Cell(i + 1).GetString().Trim();
                }
                // Skip entirely empty rows
                if (values.All(string.IsNullOrWhiteSpace)) continue;
                rows.Add(values);
            }

            if (rows.Count > 0)
                sheets.Add(new ImportSheet(ws.Name, headers, rows));
        }

        return sheets;
    }

    public static List<ImportSheet> ParseCsv(Stream stream, string fileName)
    {
        using var reader = new StreamReader(stream);
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            MissingFieldFound = null,
            BadDataFound = null,
        };
        using var csv = new CsvReader(reader, config);

        csv.Read();
        csv.ReadHeader();
        var headers = csv.HeaderRecord ?? Array.Empty<string>();
        headers = headers.Select(h => h.Trim()).ToArray();

        if (headers.Length == 0) return new();

        var rows = new List<string[]>();
        while (csv.Read())
        {
            var values = new string[headers.Length];
            for (int i = 0; i < headers.Length; i++)
            {
                values[i] = csv.GetField(i)?.Trim() ?? "";
            }
            if (values.All(string.IsNullOrWhiteSpace)) continue;
            rows.Add(values);
        }

        var sheetName = Path.GetFileNameWithoutExtension(fileName);
        if (rows.Count > 0)
            return new() { new ImportSheet(sheetName, headers, rows) };

        return new();
    }

    /// <summary>
    /// Maps spreadsheet headers to InventoryItem property names.
    /// Returns list of (header, propertyName) pairs and list of unmapped headers.
    /// </summary>
    public static (List<SharedColumnMapping> Mappings, List<string> Unmapped) MapHeaders(string[] headers)
    {
        var mappings = new List<SharedColumnMapping>();
        var usedProperties = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var unmapped = new List<string>();

        foreach (var header in headers)
        {
            var normalizedHeader = header.Replace(" ", "");

            // Try exact match (case-insensitive, strip spaces)
            var match = ItemProperties.FirstOrDefault(p =>
                !usedProperties.Contains(p) &&
                string.Equals(p, normalizedHeader, StringComparison.OrdinalIgnoreCase));

            // Try exact match on original header too (e.g. "IP" = "IP")
            if (match == null)
            {
                match = ItemProperties.FirstOrDefault(p =>
                    !usedProperties.Contains(p) &&
                    string.Equals(p, header, StringComparison.OrdinalIgnoreCase));
            }

            // Try contains match — prefer the longest matching property name to avoid
            // "Version" matching "Full Version" when "FullVersion" exists
            if (match == null)
            {
                match = ItemProperties
                    .Where(p => !usedProperties.Contains(p) &&
                        (string.Equals(p, normalizedHeader, StringComparison.OrdinalIgnoreCase) ||
                         normalizedHeader.Equals(p, StringComparison.OrdinalIgnoreCase)))
                    .FirstOrDefault();
            }

            if (match == null)
            {
                // Fuzzy: property name is contained in header or vice versa
                // Pick the longest match to be most specific
                match = ItemProperties
                    .Where(p => !usedProperties.Contains(p) &&
                        (normalizedHeader.Contains(p, StringComparison.OrdinalIgnoreCase) ||
                         p.Contains(normalizedHeader, StringComparison.OrdinalIgnoreCase)))
                    .OrderByDescending(p => p.Length)
                    .FirstOrDefault();
            }

            if (match != null)
            {
                usedProperties.Add(match);
                mappings.Add(new SharedColumnMapping { Header = header, Property = match });
            }
            else
            {
                // Assign to next available property
                var available = ItemProperties.FirstOrDefault(p => !usedProperties.Contains(p));
                if (available != null)
                {
                    usedProperties.Add(available);
                    mappings.Add(new SharedColumnMapping { Header = header, Property = available });
                }
                else
                {
                    unmapped.Add(header);
                }
            }
        }

        return (mappings, unmapped);
    }

    /// <summary>
    /// Creates InventoryItem objects from rows using the column mappings.
    /// </summary>
    public static List<InventoryItem> CreateItems(List<string[]> rows, List<SharedColumnMapping> mappings, int groupId)
    {
        var items = new List<InventoryItem>();

        foreach (var row in rows)
        {
            var item = new InventoryItem { InventoryGroupId = groupId };

            for (int i = 0; i < mappings.Count && i < row.Length; i++)
            {
                var prop = typeof(InventoryItem).GetProperty(mappings[i].Property);
                if (prop != null && prop.CanWrite)
                {
                    prop.SetValue(item, row[i]);
                }
            }

            items.Add(item);
        }

        return items;
    }
}
