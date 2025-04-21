using System.ComponentModel.DataAnnotations;

namespace Electronics.Models
{   //Estructura de Categorias
    public class Categorias
    {
        [Key]
        public int IdCategoria { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        public virtual ICollection<Productos> Productos { get; set; }
    }
}

