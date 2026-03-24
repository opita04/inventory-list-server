using InventoryList.Components;
using InventoryList.Data;
using InventoryList.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(options => options.DetailedErrors = true);

builder.Services.AddBlazorBootstrap();
builder.Services.AddScoped<ITemplateService, TemplateService>();

builder.Services.AddDbContextFactory<InventoryDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Auto-create / migrate the database on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();
    db.Database.EnsureCreated();

    if (!db.InventoryTemplates.Any(t => t.Name == "SQL Server" || t.Name == "Original Default List"))
    {
        var mappings = InventoryList.Services.ExcelPasteParser.DefaultColumnHeaders
            .Select(h => new { Header = h, Property = h })
            .ToList();
        
        db.InventoryTemplates.Add(new InventoryList.Models.InventoryTemplate
        {
            Name = "SQL Server",
            MappingsJson = System.Text.Json.JsonSerializer.Serialize(mappings)
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
