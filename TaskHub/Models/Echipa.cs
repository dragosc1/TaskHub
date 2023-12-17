using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using TaskHub.Database;

namespace TaskHub.Models
{
    public class Echipa
    {
        public string IdUtilizator { get; set; }
        public int IdProiect { get; set; }

        public virtual ApplicationUser? Utilizator { get; set; }
        public virtual Proiect? Proiect { get; set; }

    }
}
