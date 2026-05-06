using app_agenda.Test.Infraestructura;
using app_agenda.Data.Models;
using app_agenda.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace app_agenda.Test.Services;

public class CategoryServiceTest
{
    [Fact]
    public async Task Buscar_CuandoExiste_RetornaCategory()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Categories.Add(CreateCategory(id: 1, userId: 1, name: "Personal", iconCode: "User"));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new CategoryService(context);

        var result = await service.Buscar(1);

        Assert.NotNull(result);
        Assert.Equal(1, result!.Id);
        Assert.Equal("Personal", result.Name);
        Assert.Equal(1, result.UserId);
    }

    [Fact]
    public async Task Buscar_CuandoNoExiste_RetornaNull()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new CategoryService(context);

        var result = await service.Buscar(99);

        Assert.Null(result);
    }

    [Fact]
    public async Task Guardar_CuandoEsNueva_AgregaCategory()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new CategoryService(context);

        var category = CreateCategory(id: 0, userId: 1, name: "Trabajo", iconCode: "Briefcase");
        var result = await service.Guardar(category);

        Assert.True(result);
        await using var verifyContext = TestDbContextFactory.CreateContext(dbName);
        var saved = await verifyContext.Categories.FirstOrDefaultAsync(c => c.Name == "Trabajo");
        Assert.NotNull(saved);
        Assert.Equal(1, saved!.UserId);
    }

    [Fact]
    public async Task Guardar_CuandoExiste_ActualizaCategory()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Categories.Add(CreateCategory(id: 1, userId: 1, name: "Nombre original", iconCode: "User"));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new CategoryService(context);
        var category = await service.Buscar(1);
        category!.Name = "Nombre actualizado";

        var result = await service.Guardar(category);

        Assert.True(result);
        await using var verifyContext = TestDbContextFactory.CreateContext(dbName);
        var updated = await verifyContext.Categories.FindAsync(1);
        Assert.Equal("Nombre actualizado", updated!.Name);
    }

    [Fact]
    public async Task Eliminar_CuandoExiste_MarcaComoEliminada()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Categories.Add(CreateCategory(id: 1, userId: 1, name: "A eliminar", iconCode: "User"));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new CategoryService(context);

        var result = await service.Eliminar(1);

        Assert.True(result);
        await using var verifyContext = TestDbContextFactory.CreateContext(dbName);
        var deleted = await verifyContext.Categories.IgnoreQueryFilters()
            .FirstOrDefaultAsync(c => c.Id == 1);
        Assert.NotNull(deleted);
        Assert.True(deleted!.IsDeleted);
    }

    [Fact]
    public async Task Eliminar_CuandoNoExiste_RetornaFalse()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new CategoryService(context);

        var result = await service.Eliminar(99);

        Assert.False(result);
    }

    [Fact]
    public async Task GetList_CuandoHayCategories_RetornaFiltradas()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Categories.AddRange(
                CreateCategory(id: 1, userId: 1, name: "Personal", iconCode: "User"),
                CreateCategory(id: 2, userId: 2, name: "Trabajo", iconCode: "Briefcase"),
                CreateCategory(id: 3, userId: 1, name: "Familia", iconCode: "Users")
            );
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new CategoryService(context);

        var result = await service.GetList(c => c.UserId == 1);

        Assert.Equal(2, result.Count);
        Assert.All(result, c => Assert.Equal(1, c.UserId));
    }

    [Fact]
    public async Task GetCategoriesByUser_RetornaOrdenadas()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Categories.AddRange(
                CreateCategory(id: 1, userId: 1, name: "Trabajo", iconCode: "Briefcase"),
                CreateCategory(id: 2, userId: 1, name: "Familia", iconCode: "Users"),
                CreateCategory(id: 3, userId: 1, name: "Personal", iconCode: "User")
            );
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new CategoryService(context);

        var result = service.GetCategoriesByUser(1);

        Assert.Equal(3, result.Count);
        Assert.Equal("Familia", result[0].Name);
        Assert.Equal("Personal", result[1].Name);
        Assert.Equal("Trabajo", result[2].Name);
    }

    [Fact]
    public async Task GetCategoriesByUser_SoloRetornaCategoriesDelUsuario()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Categories.AddRange(
                CreateCategory(id: 1, userId: 1, name: "Personal", iconCode: "User"),
                CreateCategory(id: 2, userId: 2, name: "Trabajo", iconCode: "Briefcase")
            );
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new CategoryService(context);

        var result = service.GetCategoriesByUser(1);

        Assert.Single(result);
        Assert.Equal(1, result[0].UserId);
    }

    [Fact]
    public async Task EnsureDefaultCategories_CuandoNoTieneCategories_CreaLasCuatroDefaults()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new CategoryService(context);

        service.EnsureDefaultCategories(1);

        await using var verifyContext = TestDbContextFactory.CreateContext(dbName);
        var categories = verifyContext.Categories.Where(c => c.UserId == 1).ToList();
        Assert.Equal(4, categories.Count);
        Assert.Contains(categories, c => c.Name == "Personal");
        Assert.Contains(categories, c => c.Name == "Trabajo");
        Assert.Contains(categories, c => c.Name == "Familia");
        Assert.Contains(categories, c => c.Name == "General");
    }

    [Fact]
    public async Task EnsureDefaultCategories_NoDuplicaCategoriasExistentes()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Categories.Add(CreateCategory(id: 1, userId: 1, name: "Personal", iconCode: "User"));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new CategoryService(context);

        service.EnsureDefaultCategories(1);

        await using var verifyContext = TestDbContextFactory.CreateContext(dbName);
        var categories = verifyContext.Categories.Where(c => c.UserId == 1).ToList();
        Assert.Equal(4, categories.Count);
        Assert.Single(categories, c => c.Name == "Personal");
    }

    [Fact]
    public async Task AddCategory_CreaYRetornaCategory()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new CategoryService(context);

        var result = service.AddCategory(userId: 1, name: "Nueva categoria", iconCode: "Star");

        Assert.NotNull(result);
        Assert.Equal("Nueva categoria", result.Name);
        Assert.Equal(1, result.UserId);
        await using var verifyContext = TestDbContextFactory.CreateContext(dbName);
        var saved = await verifyContext.Categories.FirstOrDefaultAsync(c => c.Name == "Nueva categoria");
        Assert.NotNull(saved);
    }

    [Fact]
    public async Task UpdateCategory_ActualizaNombreEIcono()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Categories.Add(CreateCategory(id: 1, userId: 1, name: "Nombre original", iconCode: "User"));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new CategoryService(context);

        service.UpdateCategory(new Category { Id = 1, Name = "Nombre nuevo", IconCode = "Star", UserId = 1 });

        await using var verifyContext = TestDbContextFactory.CreateContext(dbName);
        var updated = await verifyContext.Categories.FindAsync(1);
        Assert.Equal("Nombre nuevo", updated!.Name);
        Assert.Equal("Star", updated.IconCode);
    }

    [Fact]
    public async Task DeleteCategory_MarcaComoEliminada()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Categories.Add(CreateCategory(id: 1, userId: 1, name: "A eliminar", iconCode: "User"));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new CategoryService(context);

        service.DeleteCategory(1);

        await using var verifyContext = TestDbContextFactory.CreateContext(dbName);
        var deleted = await verifyContext.Categories.IgnoreQueryFilters()
            .FirstOrDefaultAsync(c => c.Id == 1);
        Assert.NotNull(deleted);
        Assert.True(deleted!.IsDeleted);
    }

    private static Category CreateCategory(int id, int userId, string name, string iconCode, bool isDeleted = false)
    {
        return new Category
        {
            Id = id,
            Name = name,
            IconCode = iconCode,
            UserId = userId,
            IsDeleted = isDeleted,
            CreatedAt = DateTime.Now
        };
    }
}
