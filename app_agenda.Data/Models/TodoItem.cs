using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel.DataAnnotations;

namespace app_agenda.Data.Models
{
    public class TodoItem
    {
        public int Id { get; set; }

        [Required, StringLength(250)]
        public string Title { get; set; }

        public bool IsCompleted { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Relación con el usuario
        public int UserId { get; set; }
        public User User { get; set; }
    }
}