using app_agenda.Data;
using app_agenda.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace app_agenda.UI.Services
{
    public class CategoryService
    {
        // Traer todas las categorías de un usuario específico
        public List<Category> GetCategoriesByUser(int userId)
        {
            using (var db = new AgendaContext())
            {
                return db.Categories
                    .Where(c => c.UserId == userId && !c.IsDeleted)
                    .ToList();
            }
        }
    }
}