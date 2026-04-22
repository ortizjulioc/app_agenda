using app_agenda.Data;
using app_agenda.Data.Models;
using System.Linq;

namespace app_agenda.UI.Services
{
    public class AuthService
    {
        // Método para validar el login
        public User Login(string username, string password)
        {
            using (var db = new AgendaContext())
            {
                // Buscamos el usuario en la BD que coincida con nombre y clave
                return db.Users.FirstOrDefault(u => u.Username == username && u.PasswordHash == password);
            }
        }

        // Aquí podríamos agregar después: Register(), ChangePassword(), etc.
    }
}