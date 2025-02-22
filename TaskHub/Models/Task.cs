﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TaskHub.Database;

namespace TaskHub.Models
{
    public class Task
    {
        [Key]
        public int Id { get; set; }
        public int? ProiectId { get; set; }
        
        [Required(ErrorMessage = "Titlul este obligatoriu")]
        [StringLength(100)]
        public string Titlu { get; set; }

        [Required(ErrorMessage = "Descrierea este obligatorie")]
        public string Descriere { get; set; }

        [Required(ErrorMessage = "Status-ul este obligatoriu")]
        public string Status { get; set; }

        [Required(ErrorMessage = "Data de inceput este obligatorie")]
        [DataType(DataType.DateTime)]
        public DateTime DataStart { get; set; }

        [Required(ErrorMessage = "Data de finalizare este obligatorie")]
        [DataType(DataType.DateTime)]
        public DateTime DataFinalizare { get; set; }

        public string? ContinutMedia { get; set; }

        public virtual Proiect? Proiect { get; set; }

        public virtual IEnumerable<Comentariu>? Comentarii { get; set; }
        public virtual IEnumerable<ApplicationUser>? Users { get; set; }
    }
}
