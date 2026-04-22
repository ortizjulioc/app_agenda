using app_agenda.Data;
using app_agenda.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace app_agenda.UI.Services
{
    public class CategoryService
    {
        private static readonly Dictionary<string, string> DefaultCategories = new()
        {
            { "Personal", "User" },
            { "Trabajo", "Briefcase" },
            { "Familia", "Users" },
            { "General", "Folder" }
        };

        public List<Category> GetCategoriesByUser(int userId)
        {
            using var db = new AgendaContext();
            return db.Categories
                .Where(c => c.UserId == userId)
                .OrderBy(c => c.Name)
.ToList();
        }

        public void EnsureDefaultCategories(int userId)
        {
            using var db = new AgendaContext();
            var existingNames = db.Categories
                .Where(c => c.UserId == userId)
                .Select(c => c.Name)
                .ToList();

            foreach (var kvp in DefaultCategories)
            {
                if (!existingNames.Contains(kvp.Key))
                {
                    db.Categories.Add(new Category
                    {
                        Name = kvp.Key,
                        IconCode = kvp.Value,
                        UserId = userId,
                        CreatedAt = System.DateTime.Now
                    });
                }
            }
            db.SaveChanges();
        }

        public Category AddCategory(int userId, string name, string iconCode)
        {
            using var db = new AgendaContext();
            var category = new Category
            {
                Name = name,
                IconCode = iconCode,
                UserId = userId,
                CreatedAt = System.DateTime.Now
            };
            db.Categories.Add(category);
            db.SaveChanges();
            return category;
        }

        public void UpdateCategory(Category category)
        {
            using var db = new AgendaContext();
            var existing = db.Categories.Find(category.Id);
            if (existing != null)
            {
                existing.Name = category.Name;
                existing.IconCode = category.IconCode;
                db.SaveChanges();
            }
        }

        public void DeleteCategory(int id)
        {
            using var db = new AgendaContext();
            var category = db.Categories.Find(id);
            if (category != null)
            {
                category.IsDeleted = true;
                db.SaveChanges();
            }
        }
    }
}