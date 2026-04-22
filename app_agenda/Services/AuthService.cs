using app_agenda.Data;
using app_agenda.Data.Models;
using System.Linq;

namespace app_agenda.UI.Services
{
    public class AuthService
    {
        public User? Login(string username, string password)
        {
            using var db = new AgendaContext();
            var user = db.Users.FirstOrDefault(u => u.Username == username);
            if (user == null)
                return null;

            if (BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return user;

            return null;
        }

        public bool Register(string username, string password)
        {
            using var db = new AgendaContext();

            if (db.Users.Any(u => u.Username == username))
                return false;

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var newUser = new User
            {
                Username = username,
                PasswordHash = hashedPassword
            };

            db.Users.Add(newUser);
            db.SaveChanges();

            return true;
        }
    }
}