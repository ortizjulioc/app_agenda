using app_agenda.Test.Infraestructura;
using app_agenda.Data.Models;
using app_agenda.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace app_agenda.Test.Services;

public class ContactServiceTest
{
    [Fact]
    public async Task Buscar_CuandoExiste_RetornaContact()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Contacts.Add(CreateContact(id: 1, userId: 1, name: "Juan Pérez", phone: "1234567890", categoryId: 1));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new ContactService(context);

        var result = await service.Buscar(1);

        Assert.NotNull(result);
        Assert.Equal(1, result!.Id);
        Assert.Equal("Juan Pérez", result.Name);
        Assert.Equal(1, result.UserId);
    }

    [Fact]
    public async Task Buscar_CuandoNoExiste_RetornaNull()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new ContactService(context);

        var result = await service.Buscar(99);

        Assert.Null(result);
    }

    [Fact]
    public async Task Guardar_CuandoEsNuevo_AgregaContact()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new ContactService(context);

        var contact = CreateContact(id: 0, userId: 1, name: "Ana García", phone: "9876543210", categoryId: 1);
        var result = await service.Guardar(contact);

        Assert.True(result);
        await using var verifyContext = TestDbContextFactory.CreateContext(dbName);
        var saved = await verifyContext.Contacts.FirstOrDefaultAsync(c => c.Name == "Ana García");
        Assert.NotNull(saved);
        Assert.Equal(1, saved!.UserId);
    }

    [Fact]
    public async Task Guardar_CuandoExiste_ActualizaContact()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Contacts.Add(CreateContact(id: 1, userId: 1, name: "Nombre original", phone: "111", categoryId: 1));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new ContactService(context);
        var contact = await service.Buscar(1);
        contact!.Name = "Nombre actualizado";

        var result = await service.Guardar(contact);

        Assert.True(result);
        await using var verifyContext = TestDbContextFactory.CreateContext(dbName);
        var updated = await verifyContext.Contacts.FindAsync(1);
        Assert.Equal("Nombre actualizado", updated!.Name);
    }

    [Fact]
    public async Task Eliminar_CuandoExiste_MarcaComoEliminado()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Contacts.Add(CreateContact(id: 1, userId: 1, name: "A eliminar", phone: "111", categoryId: 1));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new ContactService(context);

        var result = await service.Eliminar(1);

        Assert.True(result);
        await using var verifyContext = TestDbContextFactory.CreateContext(dbName);
        var deleted = await verifyContext.Contacts.IgnoreQueryFilters()
            .FirstOrDefaultAsync(c => c.Id == 1);
        Assert.NotNull(deleted);
        Assert.True(deleted!.IsDeleted);
    }

    [Fact]
    public async Task Eliminar_CuandoNoExiste_RetornaFalse()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new ContactService(context);

        var result = await service.Eliminar(99);

        Assert.False(result);
    }

    [Fact]
    public async Task GetList_CuandoHayContactos_RetornaFiltrados()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Contacts.AddRange(
                CreateContact(id: 1, userId: 1, name: "Juan", phone: "111", categoryId: 1),
                CreateContact(id: 2, userId: 2, name: "Maria", phone: "222", categoryId: 1),
                CreateContact(id: 3, userId: 1, name: "Carlos", phone: "333", categoryId: 1)
            );
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new ContactService(context);

        var result = await service.GetList(c => c.UserId == 1);

        Assert.Equal(2, result.Count);
        Assert.All(result, c => Assert.Equal(1, c.UserId));
    }

    [Fact]
    public async Task GetContacts_RetornaOrdenadosPorNombre()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Contacts.AddRange(
                CreateContact(id: 1, userId: 1, name: "Carlos", phone: "111", categoryId: 1),
                CreateContact(id: 2, userId: 1, name: "Ana", phone: "222", categoryId: 1),
                CreateContact(id: 3, userId: 1, name: "Bruno", phone: "333", categoryId: 1)
            );
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new ContactService(context);

        var result = service.GetContacts(1);

        Assert.Equal(3, result.Count);
        Assert.Equal("Ana", result[0].Name);
        Assert.Equal("Bruno", result[1].Name);
        Assert.Equal("Carlos", result[2].Name);
    }

    [Fact]
    public async Task GetContacts_FiltradoPorCategoria()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Contacts.AddRange(
                CreateContact(id: 1, userId: 1, name: "Juan", phone: "111", categoryId: 1),
                CreateContact(id: 2, userId: 1, name: "Maria", phone: "222", categoryId: 2),
                CreateContact(id: 3, userId: 1, name: "Carlos", phone: "333", categoryId: 1)
            );
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new ContactService(context);

        var result = service.GetContacts(userId: 1, categoryId: 1);

        Assert.Equal(2, result.Count);
        Assert.All(result, c => Assert.Equal(1, c.CategoryId));
    }

    [Fact]
    public async Task GetContacts_SoloRetornaContactosDelUsuario()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Contacts.AddRange(
                CreateContact(id: 1, userId: 1, name: "Juan", phone: "111", categoryId: 1),
                CreateContact(id: 2, userId: 2, name: "Maria", phone: "222", categoryId: 1)
            );
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new ContactService(context);

        var result = service.GetContacts(1);

        Assert.Single(result);
        Assert.Equal(1, result[0].UserId);
    }

    [Fact]
    public async Task GetFavorites_RetornaContactosFavoritos()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Contacts.AddRange(
                CreateContact(id: 1, userId: 1, name: "Juan", phone: "111", categoryId: 1, isFavorite: true),
                CreateContact(id: 2, userId: 1, name: "Maria", phone: "222", categoryId: 1, isFavorite: false),
                CreateContact(id: 3, userId: 1, name: "Carlos", phone: "333", categoryId: 1, isFavorite: true)
            );
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new ContactService(context);

        var result = service.GetFavorites(1);

        Assert.Equal(2, result.Count);
        Assert.All(result, c => Assert.True(c.IsFavorite));
    }

    [Fact]
    public async Task SearchContacts_FiltraPorNombre()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Contacts.AddRange(
                CreateContact(id: 1, userId: 1, name: "Juan Pérez", phone: "111", categoryId: 1),
                CreateContact(id: 2, userId: 1, name: "Ana García", phone: "222", categoryId: 1),
                CreateContact(id: 3, userId: 1, name: "Juan López", phone: "333", categoryId: 1)
            );
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new ContactService(context);

        var result = service.SearchContacts(userId: 1, term: "juan");

        Assert.Equal(2, result.Count);
        Assert.All(result, c => Assert.Contains("Juan", c.Name));
    }

    [Fact]
    public async Task SearchContacts_FiltraPorTelefono()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Contacts.AddRange(
                CreateContact(id: 1, userId: 1, name: "Juan", phone: "1234567890", categoryId: 1),
                CreateContact(id: 2, userId: 1, name: "Ana", phone: "9876543210", categoryId: 1)
            );
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new ContactService(context);

        var result = service.SearchContacts(userId: 1, term: "1234");

        Assert.Single(result);
        Assert.Equal("Juan", result[0].Name);
    }

    [Fact]
    public async Task SearchContacts_FiltradoPorCategoria()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Contacts.AddRange(
                CreateContact(id: 1, userId: 1, name: "Juan", phone: "111", categoryId: 1),
                CreateContact(id: 2, userId: 1, name: "Juanita", phone: "222", categoryId: 2)
            );
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new ContactService(context);

        var result = service.SearchContacts(userId: 1, term: "juan", categoryId: 1);

        Assert.Single(result);
        Assert.Equal(1, result[0].CategoryId);
    }

    [Fact]
    public async Task GetById_CuandoExiste_RetornaContact()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Contacts.Add(CreateContact(id: 1, userId: 1, name: "Juan", phone: "111", categoryId: 1));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new ContactService(context);

        var result = service.GetById(1);

        Assert.NotNull(result);
        Assert.Equal("Juan", result!.Name);
    }

    [Fact]
    public async Task AddContact_AgregaContactoEnBD()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new ContactService(context);

        var contact = CreateContact(id: 0, userId: 1, name: "Nuevo contacto", phone: "555", categoryId: 1);
        service.AddContact(contact);

        await using var verifyContext = TestDbContextFactory.CreateContext(dbName);
        var saved = await verifyContext.Contacts.FirstOrDefaultAsync(c => c.Name == "Nuevo contacto");
        Assert.NotNull(saved);
        Assert.Equal(1, saved!.UserId);
    }

    [Fact]
    public async Task UpdateContact_ActualizaCamposDelContacto()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Contacts.Add(CreateContact(id: 1, userId: 1, name: "Nombre original", phone: "111", categoryId: 1));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new ContactService(context);

        service.UpdateContact(new Contact { Id = 1, Name = "Nombre nuevo", PhoneNumber = "999", CategoryId = 2, UserId = 1 });

        await using var verifyContext = TestDbContextFactory.CreateContext(dbName);
        var updated = await verifyContext.Contacts.FindAsync(1);
        Assert.Equal("Nombre nuevo", updated!.Name);
        Assert.Equal("999", updated.PhoneNumber);
        Assert.Equal(2, updated.CategoryId);
    }

    [Fact]
    public async Task ToggleFavorite_CuandoNoEsFavorito_MarcaComoFavorito()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Contacts.Add(CreateContact(id: 1, userId: 1, name: "Juan", phone: "111", categoryId: 1, isFavorite: false));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new ContactService(context);

        service.ToggleFavorite(1);

        var result = await service.Buscar(1);
        Assert.True(result!.IsFavorite);
    }

    [Fact]
    public async Task ToggleFavorite_CuandoEsFavorito_DesactivaFavorito()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Contacts.Add(CreateContact(id: 1, userId: 1, name: "Juan", phone: "111", categoryId: 1, isFavorite: true));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new ContactService(context);

        service.ToggleFavorite(1);

        var result = await service.Buscar(1);
        Assert.False(result!.IsFavorite);
    }

    [Fact]
    public async Task DeleteContact_MarcaComoEliminado()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Contacts.Add(CreateContact(id: 1, userId: 1, name: "A eliminar", phone: "111", categoryId: 1));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new ContactService(context);

        service.DeleteContact(1);

        await using var verifyContext = TestDbContextFactory.CreateContext(dbName);
        var deleted = await verifyContext.Contacts.IgnoreQueryFilters()
            .FirstOrDefaultAsync(c => c.Id == 1);
        Assert.NotNull(deleted);
        Assert.True(deleted!.IsDeleted);
    }

    private static Contact CreateContact(
        int id, int userId, string name, string phone, int categoryId,
        bool isFavorite = false, bool isDeleted = false)
    {
        return new Contact
        {
            Id = id,
            Name = name,
            PhoneNumber = phone,
            CategoryId = categoryId,
            UserId = userId,
            IsFavorite = isFavorite,
            IsDeleted = isDeleted,
            CreatedAt = DateTime.Now
        };
    }
}
