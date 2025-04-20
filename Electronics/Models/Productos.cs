using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;   // <<-- para ValidateNever

namespace Electronics.Models
{
    public class Productos
    {
        [Key]
        public int IdProducto { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que cero.")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El stock es obligatorio.")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock debe ser cero o mayor.")]
        public int Stock { get; set; }

        // foreign key a Categorías
        [ForeignKey("Categoria")]
        [Required(ErrorMessage = "La categoría es obligatoria.")]
        public int IdCategoria { get; set; }

        [ValidateNever]
        public virtual Categorias Categoria { get; set; }


        // este es el FK que apunta a tu tabla de imágenes
        public int? IdImagen { get; set; }

        [ValidateNever]
        [ForeignKey("IdImagen")]
        public virtual Imagenes Imagen { get; set; }
    }
}
