using System.ComponentModel.DataAnnotations;

namespace Electronics.Models
{
    public class Categorias
    {
        [Key]
        public int IdCategoria { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }


        public ICollection<Productos> Productos { get; set; }
    }
}
