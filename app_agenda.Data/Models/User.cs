using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel.DataAnnotations;

namespace app_agenda.Data.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Relaciones: Un usuario tiene muchas categorías, contactos y tareas
        public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
        public virtual ICollection<Contact> Contacts { get; set; } = new List<Contact>();
        public virtual ICollection<TodoItem> TodoItems { get; set; } = new List<TodoItem>();
    }
}