﻿using System.ComponentModel.DataAnnotations;

namespace TaskHub.Models
{
    public class Proiect
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Numele proiectului este obligatoriu")]
        [StringLength(100)]
        public string NumeProiect { get; set; }

        public virtual Utilizator Organizator { get; set; }

        [Required(ErrorMessage = "Descrierea proiectului este obligatorie")]
        [StringLength(500)]
        public string Descriere { get; set; }   
    }
}
