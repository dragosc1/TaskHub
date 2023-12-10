using System.ComponentModel.DataAnnotations;

namespace TaskHub.Models
{
    public class Echipa
    {
        public int IdUtilizator { get; set; }
        public int IdProiect { get; set; }

        public virtual Utilizator Utilizator { get; set; }
        public virtual Proiect Proiect { get; set; }
    }
}
