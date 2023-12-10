using System.ComponentModel.DataAnnotations;

namespace TaskHub.Models
{
    public class Comentariu
    {
        public int? Id { get; set; }
        public int? IdUtilizator { get; set; }   
        public int? IdTask { get; set; }

        [Required(ErrorMessage = "Continutul comentariului este obligatoriu")]
        [StringLength(500)]
        public string Continut { get; set; }

        public virtual Utilizator Utilizator { get; set; }
        public virtual Task Task { get; set; }
    }
}
