using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using TaskHub.Models;

namespace TaskHub.Database
{
    public class ApplicationUser : IdentityUser
    {
        public string? Nume { get; set; }

        [StringLength(100)]
        public string? Prenume { get; set; }

        public virtual IEnumerable<Echipa>? Echipe { get; set; }

        public virtual IEnumerable<Comentariu>? Comentarii { get; set; }

        public virtual IEnumerable<Models.Task>? Tasks { get; set; }
    }
}
