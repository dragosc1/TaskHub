using System.ComponentModel.DataAnnotations;
using TaskHub.Database;

namespace TaskHub.Models
{
    public class Comentariu
    {
        public int Id { get; set; }
        public string IdUtilizator { get; set; }   
        public int IdTask { get; set; }

        [Required(ErrorMessage = "Continutul comentariului este obligatoriu")]
        [StringLength(500)]
        public string Continut { get; set; }

        public virtual ApplicationUser Utilizator { get; set; }
        public virtual Task Task { get; set; }
    }
}
