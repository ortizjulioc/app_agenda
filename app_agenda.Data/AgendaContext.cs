using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using app_agenda.Data.Models;

namespace app_agenda.Data
{
    public class AgendaContext : DbContext
    {
        // Estas propiedades representan tus tablas en la BD
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<TodoItem> TodoItems { get; set; }

        // Configuración de la conexión
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Hemos añadido 'Initial Catalog=agenda_bd' para que sepa a qué base entrar
                optionsBuilder.UseSqlServer(@"Data Source=.\SQLEXPRESS;Initial Catalog=agenda_bd;Integrated Security=True;TrustServerCertificate=True;");
            }
        }

        // Aquí configuramos el "Soft Delete" global
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Filtro para no traer nunca los registros que tengan IsDeleted = true
            modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
            modelBuilder.Entity<Category>().HasQueryFilter(c => !c.IsDeleted);
            modelBuilder.Entity<Contact>().HasQueryFilter(c => !c.IsDeleted);
            modelBuilder.Entity<TodoItem>().HasQueryFilter(t => !t.IsDeleted);

            // Puedes agregar configuraciones adicionales aquí si lo necesitas
        }
    }
}