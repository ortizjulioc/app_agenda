using app_agenda.Data;
using app_agenda.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace app_agenda.UI.Services;

public class TodoService : Aplicada1.Core.IService<TodoItem, int>
{
    private readonly AgendaContext _db;

    public TodoService(AgendaContext db) => _db = db;

    public TodoService() : this(new AgendaContext()) { }

    public Task<bool> Guardar(TodoItem entidad)
    {
        try
        {
            if (entidad.Id == 0)
                _db.TodoItems.Add(entidad);
            else
                _db.TodoItems.Update(entidad);
            _db.SaveChanges();
            return Task.FromResult(true);
        }
        catch { return Task.FromResult(false); }
    }

    public Task<TodoItem?> Buscar(int id)
    {
        return Task.FromResult(_db.TodoItems.Find(id));
    }

    public Task<bool> Eliminar(int id)
    {
        try
        {
            var todo = _db.TodoItems.Find(id);
            if (todo == null) return Task.FromResult(false);
            todo.IsDeleted = true;
            _db.SaveChanges();
            return Task.FromResult(true);
        }
        catch { return Task.FromResult(false); }
    }

    public Task<List<TodoItem>> GetList(Expression<Func<TodoItem, bool>> criterio)
    {
        return Task.FromResult(_db.TodoItems.Where(criterio).ToList());
    }

    // ── Métodos de mios  ───────────────

    public List<TodoItem> GetTodos(int userId)
    {
        return GetList(t => t.UserId == userId)
            .GetAwaiter().GetResult()
            .OrderBy(t => t.IsCompleted)
            .ThenByDescending(t => t.CreatedAt)
            .ToList();
    }

    public TodoItem AddTodo(int userId, string title)
    {
        var todo = new TodoItem
        {
            Title = title,
            UserId = userId,
            IsCompleted = false,
            CreatedAt = DateTime.Now
        };
        Guardar(todo).GetAwaiter().GetResult();
        return todo;
    }

    public void ToggleTodo(int id)
    {
        var todo = Buscar(id).GetAwaiter().GetResult();
        if (todo == null) return;
        todo.IsCompleted = !todo.IsCompleted;
        Guardar(todo).GetAwaiter().GetResult();
    }

    public void DeleteTodo(int id) => Eliminar(id).GetAwaiter().GetResult();
}