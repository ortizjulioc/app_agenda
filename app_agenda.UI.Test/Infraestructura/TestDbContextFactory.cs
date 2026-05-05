using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using app_agenda.Data;


namespace app_agenda.Test.Infraestructura;

public static class TestDbContextFactory
{
    public static string NewDatabaseName() => $"agenda_bd_{Guid.NewGuid()}";

    public static AgendaContext CreateContext(string databaseName)
    {
        var options = new DbContextOptionsBuilder<AgendaContext>()
            .UseInMemoryDatabase(databaseName)
            .Options;

        return new InMemoryAgendaContext(options);
    }

    private sealed class InMemoryAgendaContext(DbContextOptions<AgendaContext> options)
        : AgendaContext(options)
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Intentionally empty: tests provide InMemory provider through options.
        }
    }

}
