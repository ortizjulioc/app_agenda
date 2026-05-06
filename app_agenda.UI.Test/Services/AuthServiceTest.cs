using app_agenda.Test.Infraestructura;
using app_agenda.Data.Models;
using app_agenda.UI.Services;

namespace app_agenda.Test.Services;

public class AuthServiceTest
{
    [Fact]
    public async Task Login_CuandoCredencialesCorrectas_RetornaUser()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword("contraseña123");
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Users.Add(CreateUser(id: 1, username: "usuario1", passwordHash: hashedPassword));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new AuthService(context);

        var result = service.Login("usuario1", "contraseña123");

        Assert.NotNull(result);
        Assert.Equal("usuario1", result!.Username);
    }

    [Fact]
    public async Task Login_CuandoPasswordIncorrecta_RetornaNull()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword("contraseña123");
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Users.Add(CreateUser(id: 1, username: "usuario1", passwordHash: hashedPassword));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new AuthService(context);

        var result = service.Login("usuario1", "passwordIncorrecta");

        Assert.Null(result);
    }

    [Fact]
    public async Task Login_CuandoUsuarioNoExiste_RetornaNull()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new AuthService(context);

        var result = service.Login("noexiste", "cualquier");

        Assert.Null(result);
    }

    [Fact]
    public async Task Register_CuandoUsuarioNuevo_RetornaTrue()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new AuthService(context);

        var result = service.Register("nuevoUsuario", "pass123");

        Assert.True(result);
        await using var verifyContext = TestDbContextFactory.CreateContext(dbName);
        var user = verifyContext.Users.FirstOrDefault(u => u.Username == "nuevoUsuario");
        Assert.NotNull(user);
    }

    [Fact]
    public async Task Register_CuandoUsuarioYaExiste_RetornaFalse()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Users.Add(CreateUser(id: 1, username: "existente", passwordHash: "hash"));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new AuthService(context);

        var result = service.Register("existente", "cualquierPass");

        Assert.False(result);
    }

    [Fact]
    public async Task Register_GuardaPasswordHasheada()
    {
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new AuthService(context);

        service.Register("usuario", "miPassword");

        await using var verifyContext = TestDbContextFactory.CreateContext(dbName);
        var user = verifyContext.Users.FirstOrDefault(u => u.Username == "usuario");
        Assert.NotNull(user);
        Assert.NotEqual("miPassword", user!.PasswordHash);
        Assert.True(BCrypt.Net.BCrypt.Verify("miPassword", user.PasswordHash));
    }

    private static User CreateUser(int id, string username, string passwordHash)
    {
        return new User
        {
            Id = id,
            Username = username,
            PasswordHash = passwordHash,
            IsDeleted = false,
            CreatedAt = DateTime.Now
        };
    }
}
