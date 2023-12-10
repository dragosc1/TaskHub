using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using TaskHub.Database;

namespace TaskHub.Models
{
    public class Utilizator
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Numele este obligatoriu")]
        public string Nume { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Prenumele este obligatoriu")]
        public string Prenume { get; set; }
        
        [Required(ErrorMessage = "Emailul este obligatoriu")]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Telefonul este obligatoriu")]
        [Phone]
        public string Telefon { get; set; } 

        public virtual IEnumerable<Echipa>? Echipe { get; set; }
        
        public virtual IEnumerable<Comentariu>? Comentarii { get; set; }

        public virtual ApplicationUser User { get; set; }   
        public virtual IdentityRole Role { get; set; }  
    }
}
