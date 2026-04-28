using app_agenda.Data;
using app_agenda.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace app_agenda.UI.Services;

public class TodoService: Aplicada1.Core.IService<TodoItem, int>
{
    public List<TodoItem> GetTodos(int userId)
    {
        using var db = new AgendaContext();
        return db.TodoItems
            .Where(t => t.UserId == userId)
            .OrderBy(t => t.IsCompleted)
            .ThenByDescending(t => t.CreatedAt)
            .ToList();
    }

    public TodoItem AddTodo(int userId, string title)
    {
        using var db = new AgendaContext();
        var todo = new TodoItem
        {
            Title = title,
            UserId = userId,
            IsCompleted = false,
            CreatedAt = System.DateTime.Now
        };
        db.TodoItems.Add(todo);
        db.SaveChanges();
        return todo;
    }

    public void ToggleTodo(int id)
    {
        using var db = new AgendaContext();
        var todo = db.TodoItems.Find(id);
        if (todo != null)
        {
            todo.IsCompleted = !todo.IsCompleted;
            db.SaveChanges();
        }
    }

    public void DeleteTodo(int id)
    {
        using var db = new AgendaContext();
        var todo = db.TodoItems.Find(id);
        if (todo != null)
        {
            todo.IsDeleted = true;
            db.SaveChanges();
        }
    }

    public Task<bool> Guardar(TodoItem entidad)
    {
        throw new NotImplementedException();
    }

    public Task<TodoItem?> Buscar(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Eliminar(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<TodoItem>> GetList(Expression<Func<TodoItem, bool>> criterio)
    {
        throw new NotImplementedException();
    }
}