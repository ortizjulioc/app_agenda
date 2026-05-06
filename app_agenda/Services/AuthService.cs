using app_agenda.Data;
using app_agenda.Data.Models;
using System.Linq;

namespace app_agenda.UI.Services;

public class AuthService
{
    private readonly AgendaContext _db;

    public AuthService(AgendaContext db) => _db = db;

    public AuthService() : this(new AgendaContext()) { }

    public User? Login(string username, string password)
    {
        var user = _db.Users.FirstOrDefault(u => u.Username == username);
        if (user == null) return null;
        if (BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            return user;
        return null;
    }

    public bool Register(string username, string password)
    {
        if (_db.Users.Any(u => u.Username == username))
            return false;

        var newUser = new User
        {
            Username = username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
        };
        _db.Users.Add(newUser);
        _db.SaveChanges();
        return true;
    }
}
