using InventoryList.Components;
using InventoryList.Data;
using InventoryList.Models;
using InventoryList.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(options => options.DetailedErrors = true);

builder.Services.AddBlazorBootstrap();
builder.Services.AddScoped<ITemplateService, TemplateService>();

builder.Services.AddDbContextFactory<InventoryDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// ── Built-in SQL Server template (always synced from code) ──
var sqlServerMappings = new List<SharedColumnMapping>
{
    new() { Header = "Name",              Property = "Name" },
    new() { Header = "Notes/Status",      Property = "Notes" },
    new() { Header = "IP",                Property = "IP" },
    new() { Header = "Domain",            Property = "Domain" },
    new() { Header = "Onboarded",         Property = "Onboarded" },
    new() { Header = "Recovery Process",  Property = "RecoveryProcess" },
    new() { Header = "RAM",               Property = "RAM" },
    new() { Header = "Cores",             Property = "Cores" },
    new() { Header = "Version",           Property = "Version",
        SourceProperty = "FullVersion",   ExtractRegex = @"Microsoft SQL Server (\d{4})" },
    new() { Header = "Edition",           Property = "Edition",
        SourceProperty = "FullVersion",   ExtractRegex = @"(Enterprise Edition|Standard Edition|Developer Edition|Express Edition|Web Edition)" },
    new() { Header = "Full Version",      Property = "FullVersion" },
    new() { Header = "CU Version",        Property = "CUVersion",
        SourceProperty = "FullVersion",   ExtractRegex = @"(RTM-CU\d+|CU\d+)" },
    new() { Header = "KB Version",        Property = "KBVersion",
        SourceProperty = "FullVersion",   ExtractRegex = @"(KB\d+)" },
    new() { Header = "Last Patched On",   Property = "LastPatchedOn" },
    new() { Header = "Environment",       Property = "Environment" },
    new() { Header = "OS",                Property = "OS",
        SourceProperty = "FullVersion",   ExtractRegex = @"on (Windows Server \d{4}\s*\w*)" },
    new() { Header = "Product",           Property = "Product" },
    new() { Header = "Owner",             Property = "Owner" },
    new() { Header = "Status Notes",      Property = "StatusNotes" },
};
var sqlServerMappingsJson = JsonSerializer.Serialize(sqlServerMappings);

// Auto-create / migrate the database on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();
    db.Database.EnsureCreated();

    // Seed only: create the SQL Server template if it doesn't exist, never overwrite user edits
    var existing = db.InventoryTemplates.FirstOrDefault(t => t.Name == "SQL Server");
    if (existing == null)
    {
        db.InventoryTemplates.Add(new InventoryTemplate
        {
            Name = "SQL Server",
            MappingsJson = sqlServerMappingsJson
        });
        db.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
