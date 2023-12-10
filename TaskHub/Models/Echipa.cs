using System.ComponentModel.DataAnnotations;

namespace TaskHub.Models
{
    public class Echipa
    {
        public int? IdUtilizator { get; set; }
        public int? IdProiect { get; set; }

        [StringLength(500)]
        [Required(ErrorMessage = "Rolul in proiect este obligatoriu")]
        public string RolInProiect { get; set; }

        public virtual Utilizator Utilizator { get; set; }
        public virtual Proiect Proiect { get; set; }

    }
}
