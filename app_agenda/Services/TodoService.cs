using app_agenda.Data;
using app_agenda.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace app_agenda.UI.Services;

public class TodoService : Aplicada1.Core.IService<TodoItem, int>
{
    // ── IService<TodoItem, int> ──────────────────────────────────────
    // Operaciones síncronas envueltas en Task.FromResult para evitar
    // deadlocks al llamar con .GetAwaiter().GetResult() desde el hilo UI.

    public Task<bool> Guardar(TodoItem entidad)
    {
        try
        {
            using var db = new AgendaContext();
            if (entidad.Id == 0)
                db.TodoItems.Add(entidad);
            else
                db.TodoItems.Update(entidad);
            db.SaveChanges();
            return Task.FromResult(true);
        }
        catch { return Task.FromResult(false); }
    }

    public Task<TodoItem?> Buscar(int id)
    {
        using var db = new AgendaContext();
        return Task.FromResult(db.TodoItems.Find(id));
    }

    public Task<bool> Eliminar(int id)
    {
        try
        {
            using var db = new AgendaContext();
            var todo = db.TodoItems.Find(id);
            if (todo == null) return Task.FromResult(false);
            todo.IsDeleted = true;
            db.SaveChanges();
            return Task.FromResult(true);
        }
        catch { return Task.FromResult(false); }
    }

    public Task<List<TodoItem>> GetList(Expression<Func<TodoItem, bool>> criterio)
    {
        using var db = new AgendaContext();
        return Task.FromResult(db.TodoItems.Where(criterio).ToList());
    }

    // ── Métodos de la aplicación (delegan en IService) ───────────────

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