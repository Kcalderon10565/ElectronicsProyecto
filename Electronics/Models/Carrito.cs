
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Electronics.Models
{
    //Estructura del Carrito
    [Table("Carrito")]
    public class Carrito
    {
        [Key]
        public int IdCarrito { get; set; }

        public int IdUsuario { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }

        [ForeignKey(nameof(IdUsuario))]
        public virtual Usuario Usuario { get; set; }

        [ForeignKey(nameof(IdProducto))]
        public virtual Productos Producto { get; set; }
    }
}
