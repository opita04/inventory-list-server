using InventoryList.Data;
using InventoryList.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryList.Services;

public interface ITemplateService
{
    Task<List<InventoryTemplate>> GetTemplatesAsync();
    Task<InventoryTemplate?> GetTemplateByIdAsync(int id);
    Task<InventoryTemplate> SaveTemplateAsync(InventoryTemplate template);
    Task DeleteTemplateAsync(int id);
}

public class TemplateService : ITemplateService
{
    private readonly IDbContextFactory<InventoryDbContext> _dbFactory;

    public TemplateService(IDbContextFactory<InventoryDbContext> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task<List<InventoryTemplate>> GetTemplatesAsync()
    {
        using var context = _dbFactory.CreateDbContext();
        return await context.InventoryTemplates.Where(t => !t.IsDeleted).ToListAsync();
    }

    public async Task<InventoryTemplate?> GetTemplateByIdAsync(int id)
    {
        using var context = _dbFactory.CreateDbContext();
        return await context.InventoryTemplates.FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<InventoryTemplate> SaveTemplateAsync(InventoryTemplate template)
    {
        using var context = _dbFactory.CreateDbContext();
        if (template.Id == 0)
        {
            context.InventoryTemplates.Add(template);
        }
        else
        {
            context.Entry(template).State = EntityState.Modified;
        }
        await context.SaveChangesAsync();
        return template;
    }

    public async Task DeleteTemplateAsync(int id)
    {
        using var context = _dbFactory.CreateDbContext();
        var template = await context.InventoryTemplates.FindAsync(id);
        if (template != null)
        {
            template.IsDeleted = true;
            context.InventoryTemplates.Update(template);
            await context.SaveChangesAsync();
        }
    }
}
