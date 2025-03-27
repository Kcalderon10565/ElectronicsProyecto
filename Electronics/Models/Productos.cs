using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Electronics.Models
{
    public class Productos
    {
        [Key]
        public int IdProducto { get; set; }

        [Required]
        [ForeignKey("Categoria")]
        public int IdCategoria { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        [Required]
        public decimal Precio { get; set; }

        [Required]
        public int Stock { get; set; }

        public int? IdImagen { get; set; }


        [ForeignKey("IdImagen")]
        public virtual Imagenes Imagen { get; set; }

        [ForeignKey("IdCategoria")]
        public virtual Categorias Categoria { get; set; }
    }
}
