﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using TaskHub.Database;

namespace TaskHub.Models
{
    public class Echipa
    {
        public string IdUtilizator { get; set; }
        public int IdProiect { get; set; }

        [StringLength(500)]
        [Required(ErrorMessage = "Rolul in proiect este obligatoriu")]
        public virtual IdentityRole RolInProiect { get; set; }

        public virtual ApplicationUser? Utilizator { get; set; }
        public virtual Proiect? Proiect { get; set; }

    }
}
