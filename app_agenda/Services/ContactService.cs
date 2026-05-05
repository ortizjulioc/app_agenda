using app_agenda.Data;
using app_agenda.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace app_agenda.UI.Services;

public class ContactService:Aplicada1.Core.IService<Contact, int>
{
    public List<Contact> GetContacts(int userId, int? categoryId = null)
    {
        using var db = new AgendaContext();
        var query = db.Contacts.Where(c => c.UserId == userId);

        if (categoryId.HasValue)
            query = query.Where(c => c.CategoryId == categoryId.Value);

        return query.OrderBy(c => c.Name).ToList();
    }

    public List<Contact> GetFavorites(int userId)
    {
        using var db = new AgendaContext();
        return db.Contacts
            .Where(c => c.UserId == userId && c.IsFavorite)
            .OrderBy(c => c.Name)
            .ToList();
    }

    public List<Contact> SearchContacts(int userId, string term, int? categoryId = null)
    {
        using var db = new AgendaContext();
        var lowerTerm = term.ToLower();
        var query = db.Contacts.Where(c => c.UserId == userId &&
            (c.Name.ToLower().Contains(lowerTerm) || c.PhoneNumber.Contains(lowerTerm)));

        if (categoryId.HasValue)
            query = query.Where(c => c.CategoryId == categoryId.Value);

        return query.OrderBy(c => c.Name).ToList();
    }

    public Contact? GetById(int id)
    {
        using var db = new AgendaContext();
        return db.Contacts.Find(id);
    }

    public void AddContact(Contact contact)
    {
        using var db = new AgendaContext();
        contact.CreatedAt = System.DateTime.Now;
        db.Contacts.Add(contact);
        db.SaveChanges();
    }

    public void UpdateContact(Contact contact)
    {
        using var db = new AgendaContext();
        var existing = db.Contacts.Find(contact.Id);
        if (existing != null)
        {
            existing.Name = contact.Name;
            existing.PhoneNumber = contact.PhoneNumber;
            existing.CategoryId = contact.CategoryId;
            existing.IsFavorite = contact.IsFavorite;
            db.SaveChanges();
        }
    }

    public void ToggleFavorite(int id)
    {
        using var db = new AgendaContext();
        var contact = db.Contacts.Find(id);
        if (contact != null)
        {
            contact.IsFavorite = !contact.IsFavorite;
            db.SaveChanges();
        }
    }

    public void DeleteContact(int id)
    {
        using var db = new AgendaContext();
        var contact = db.Contacts.Find(id);
        if (contact != null)
        {
            contact.IsDeleted = true;
            db.SaveChanges();
        }
    }
    //------------------------------------------------------------------------------------
    public Task<bool> Guardar(Contact entidad)
    {
        throw new NotImplementedException();
    }

    public Task<Contact?> Buscar(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Eliminar(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Contact>> GetList(Expression<Func<Contact, bool>> criterio)
    {
        throw new NotImplementedException();
    }
}