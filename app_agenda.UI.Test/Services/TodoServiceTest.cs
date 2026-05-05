using app_agenda.Test.Infraestructura;
using app_agenda.Data.Models;
using app_agenda.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace app_agenda.Test.Services;

public class TodoServiceTest
{
    [Fact]
    public async Task Buscar_CuandoExiste_RetornaTodoItem()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.TodoItems.Add(CreateTodoItem(id: 1, userId: 1, title: "Tarea de prueba"));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new TodoService(context);

        var result = await service.Buscar(1);

        Assert.NotNull(result);
        Assert.Equal(1, result!.Id);
        Assert.Equal("Tarea de prueba", result.Title);
        Assert.Equal(1, result.UserId);
    }

    [Fact]
    public async Task Buscar_CuandoNoExiste_RetornaNull()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new TodoService(context);

        var result = await service.Buscar(99);

        Assert.Null(result);
    }

    [Fact]
    public async Task Guardar_CuandoEsNuevo_AgregaItem()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new TodoService(context);

        var todo = CreateTodoItem(id: 0, userId: 1, title: "Nueva tarea");
        var result = await service.Guardar(todo);

        Assert.True(result);
        await using var verifyContext = TestDbContextFactory.CreateContext(dbName);
        var saved = await verifyContext.TodoItems.FirstOrDefaultAsync(t => t.Title == "Nueva tarea");
        Assert.NotNull(saved);
        Assert.Equal(1, saved!.UserId);
        Assert.False(saved.IsCompleted);
    }

    [Fact]
    public async Task Guardar_CuandoExiste_ActualizaItem()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.TodoItems.Add(CreateTodoItem(id: 1, userId: 1, title: "Titulo original"));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new TodoService(context);
        var todo = await service.Buscar(1);
        todo!.Title = "Titulo actualizado";

        var result = await service.Guardar(todo);

        Assert.True(result);
        await using var verifyContext = TestDbContextFactory.CreateContext(dbName);
        var updated = await verifyContext.TodoItems.FindAsync(1);
        Assert.Equal("Titulo actualizado", updated!.Title);
    }

    [Fact]
    public async Task Eliminar_CuandoExiste_MarcaComoEliminado()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.TodoItems.Add(CreateTodoItem(id: 1, userId: 1, title: "Tarea a eliminar"));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new TodoService(context);

        var result = await service.Eliminar(1);

        Assert.True(result);
        await using var verifyContext = TestDbContextFactory.CreateContext(dbName);
        var deleted = await verifyContext.TodoItems.IgnoreQueryFilters()
            .FirstOrDefaultAsync(t => t.Id == 1);
        Assert.NotNull(deleted);
        Assert.True(deleted!.IsDeleted);
    }

    [Fact]
    public async Task Eliminar_CuandoNoExiste_RetornaFalse()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new TodoService(context);

        var result = await service.Eliminar(99);

        Assert.False(result);
    }

    [Fact]
    public async Task GetList_CuandoHayItems_RetornaFiltradosPorCriterio()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.TodoItems.AddRange(
                CreateTodoItem(id: 1, userId: 1, title: "Tarea usuario 1"),
                CreateTodoItem(id: 2, userId: 2, title: "Tarea usuario 2"),
                CreateTodoItem(id: 3, userId: 1, title: "Otra tarea usuario 1")
            );
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new TodoService(context);

        var result = await service.GetList(t => t.UserId == 1);

        Assert.Equal(2, result.Count);
        Assert.All(result, t => Assert.Equal(1, t.UserId));
    }

    [Fact]
    public async Task GetTodos_RetornaOrdenadosPendientesPrimeroLuegoMasRecientes()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        var now = DateTime.Now;
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.TodoItems.AddRange(
                CreateTodoItem(id: 1, userId: 1, title: "Completada", isCompleted: true, createdAt: now.AddMinutes(-10)),
                CreateTodoItem(id: 2, userId: 1, title: "Pendiente reciente", isCompleted: false, createdAt: now),
                CreateTodoItem(id: 3, userId: 1, title: "Pendiente antigua", isCompleted: false, createdAt: now.AddMinutes(-5))
            );
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new TodoService(context);

        var result = service.GetTodos(1);

        Assert.Equal(3, result.Count);
        Assert.False(result[0].IsCompleted);
        Assert.False(result[1].IsCompleted);
        Assert.True(result[2].IsCompleted);
        Assert.Equal("Pendiente reciente", result[0].Title);
        Assert.Equal("Pendiente antigua", result[1].Title);
    }

    [Fact]
    public async Task GetTodos_SoloRetornaTodosDelUsuario()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.TodoItems.AddRange(
                CreateTodoItem(id: 1, userId: 1, title: "Tarea usuario 1"),
                CreateTodoItem(id: 2, userId: 2, title: "Tarea usuario 2")
            );
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new TodoService(context);

        var result = service.GetTodos(1);

        Assert.Single(result);
        Assert.Equal(1, result[0].UserId);
    }

    [Fact]
    public async Task AddTodo_CreaYRetornaNuevoItem()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new TodoService(context);

        var todo = service.AddTodo(userId: 1, title: "Nueva tarea creada");

        Assert.NotNull(todo);
        Assert.Equal("Nueva tarea creada", todo.Title);
        Assert.Equal(1, todo.UserId);
        Assert.False(todo.IsCompleted);
        await using var verifyContext = TestDbContextFactory.CreateContext(dbName);
        var saved = await verifyContext.TodoItems.FirstOrDefaultAsync(t => t.Title == "Nueva tarea creada");
        Assert.NotNull(saved);
    }

    [Fact]
    public async Task ToggleTodo_CuandoEstaIncompleta_MarcaComoCompletada()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.TodoItems.Add(CreateTodoItem(id: 1, userId: 1, title: "Tarea", isCompleted: false));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new TodoService(context);

        service.ToggleTodo(1);

        var result = await service.Buscar(1);
        Assert.True(result!.IsCompleted);
    }

    [Fact]
    public async Task ToggleTodo_CuandoEstaCompleta_MarcaComoIncompleta()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.TodoItems.Add(CreateTodoItem(id: 1, userId: 1, title: "Tarea", isCompleted: true));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new TodoService(context);

        service.ToggleTodo(1);

        var result = await service.Buscar(1);
        Assert.False(result!.IsCompleted);
    }

    private static TodoItem CreateTodoItem(
        int id, int userId, string title,
        bool isCompleted = false,
        DateTime? createdAt = null)
    {
        return new TodoItem
        {
            Id = id,
            Title = title,
            UserId = userId,
            IsCompleted = isCompleted,
            IsDeleted = false,
            CreatedAt = createdAt ?? DateTime.Now
        };
    }
}
