using app_agenda.Data;
using app_agenda.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace app_agenda.UI.Services;

public class ContactService : Aplicada1.Core.IService<Contact, int>
{
    private readonly AgendaContext _db;

    public ContactService(AgendaContext db) => _db = db;

    public ContactService() : this(new AgendaContext()) { }

    public Task<bool> Guardar(Contact entidad)
    {
        try
        {
            if (entidad.Id == 0)
                _db.Contacts.Add(entidad);
            else
                _db.Contacts.Update(entidad);
            _db.SaveChanges();
            return Task.FromResult(true);
        }
        catch { return Task.FromResult(false); }
    }

    public Task<Contact?> Buscar(int id)
    {
        return Task.FromResult(_db.Contacts.Find(id));
    }

    public Task<bool> Eliminar(int id)
    {
        try
        {
            var contact = _db.Contacts.Find(id);
            if (contact == null) return Task.FromResult(false);
            contact.IsDeleted = true;
            _db.SaveChanges();
            return Task.FromResult(true);
        }
        catch { return Task.FromResult(false); }
    }

    public Task<List<Contact>> GetList(Expression<Func<Contact, bool>> criterio)
    {
        return Task.FromResult(_db.Contacts.Where(criterio).ToList());
    }

    // ── Métodos de mios ───────────────

    public List<Contact> GetContacts(int userId, int? categoryId = null)
    {
        var query = _db.Contacts.Where(c => c.UserId == userId);
        if (categoryId.HasValue)
            query = query.Where(c => c.CategoryId == categoryId.Value);
        return query.OrderBy(c => c.Name).ToList();
    }

    public List<Contact> GetFavorites(int userId)
        => _db.Contacts
            .Where(c => c.UserId == userId && c.IsFavorite)
            .OrderBy(c => c.Name)
            .ToList();

    public List<Contact> SearchContacts(int userId, string term, int? categoryId = null)
    {
        var lowerTerm = term.ToLower();
        var query = _db.Contacts.Where(c => c.UserId == userId &&
            (c.Name.ToLower().Contains(lowerTerm) || c.PhoneNumber.Contains(lowerTerm)));
        if (categoryId.HasValue)
            query = query.Where(c => c.CategoryId == categoryId.Value);
        return query.OrderBy(c => c.Name).ToList();
    }

    public Contact? GetById(int id) => Buscar(id).GetAwaiter().GetResult();

    public void AddContact(Contact contact)
    {
        contact.CreatedAt = DateTime.Now;
        Guardar(contact).GetAwaiter().GetResult();
    }

    public void UpdateContact(Contact contact)
    {
        var existing = Buscar(contact.Id).GetAwaiter().GetResult();
        if (existing != null)
        {
            existing.Name = contact.Name;
            existing.PhoneNumber = contact.PhoneNumber;
            existing.CategoryId = contact.CategoryId;
            existing.IsFavorite = contact.IsFavorite;
            _db.SaveChanges();
        }
    }

    public void ToggleFavorite(int id)
    {
        var contact = Buscar(id).GetAwaiter().GetResult();
        if (contact != null)
        {
            contact.IsFavorite = !contact.IsFavorite;
            _db.SaveChanges();
        }
    }

    public void DeleteContact(int id) => Eliminar(id).GetAwaiter().GetResult();
}
