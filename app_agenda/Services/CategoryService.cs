using app_agenda.Data;
using app_agenda.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace app_agenda.UI.Services;

public class CategoryService : Aplicada1.Core.IService<Category, int>
{
    private static readonly Dictionary<string, string> DefaultCategories = new()
    {
        { "Personal", "User" },
        { "Trabajo", "Briefcase" },
        { "Familia", "Users" },
        { "General", "Folder" }
    };

    private readonly AgendaContext _db;

    public CategoryService(AgendaContext db) => _db = db;

    public CategoryService() : this(new AgendaContext()) { }

    public Task<bool> Guardar(Category entidad)
    {
        try
        {
            if (entidad.Id == 0)
                _db.Categories.Add(entidad);
            else
                _db.Categories.Update(entidad);
            _db.SaveChanges();
            return Task.FromResult(true);
        }
        catch { return Task.FromResult(false); }
    }

    public Task<Category?> Buscar(int id)
    {
        return Task.FromResult(_db.Categories.Find(id));
    }

    public Task<bool> Eliminar(int id)
    {
        try
        {
            var category = _db.Categories.Find(id);
            if (category == null) return Task.FromResult(false);
            category.IsDeleted = true;
            _db.SaveChanges();
            return Task.FromResult(true);
        }
        catch { return Task.FromResult(false); }
    }

    public Task<List<Category>> GetList(Expression<Func<Category, bool>> criterio)
    {
        return Task.FromResult(_db.Categories.Where(criterio).ToList());
    }

    // ── Métodos de mios ───────────────

    public List<Category> GetCategoriesByUser(int userId)
        => _db.Categories.Where(c => c.UserId == userId && !c.IsDeleted).OrderBy(c => c.Name).ToList();

    public bool CategoryExists(int userId, string name, int? excludeId = null)
        => _db.Categories.Any(c =>
            c.UserId == userId &&
            !c.IsDeleted &&
            c.Name.ToLower() == name.ToLower() &&
            (excludeId == null || c.Id != excludeId));

    public void EnsureDefaultCategories(int userId)
    {
        var existingNames = _db.Categories
            .Where(c => c.UserId == userId)
            .Select(c => c.Name)
            .ToList();

        foreach (var kvp in DefaultCategories)
        {
            if (!existingNames.Contains(kvp.Key))
            {
                _db.Categories.Add(new Category
                {
                    Name = kvp.Key,
                    IconCode = kvp.Value,
                    UserId = userId,
                    CreatedAt = DateTime.Now
                });
            }
        }
        _db.SaveChanges();
    }

    public Category AddCategory(int userId, string name, string iconCode)
    {
        var category = new Category
        {
            Name = name,
            IconCode = iconCode,
            UserId = userId,
            CreatedAt = DateTime.Now
        };
        Guardar(category).GetAwaiter().GetResult();
        return category;
    }

    public void UpdateCategory(Category category)
    {
        var existing = Buscar(category.Id).GetAwaiter().GetResult();
        if (existing != null)
        {
            existing.Name = category.Name;
            existing.IconCode = category.IconCode;
            _db.SaveChanges();
        }
    }

    public void DeleteCategory(int id) => Eliminar(id).GetAwaiter().GetResult();
}
