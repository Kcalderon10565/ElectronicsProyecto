using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Electronics.Models
{
    //Representa un pedido, en donde se registran los atributos mencionados.
    [Table("Pedidos")]
    public class Pedido
    {
        [Key]
        public int IdPedido { get; set; }

        public int IdUsuario { get; set; }

       
        [Required]
        public string Fecha { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        [Required, StringLength(16)]
        public string NumeroTarjeta { get; set; }

        [Required]
        public string FechaVencimiento { get; set; }

        [Required, StringLength(3)]
        public string CCV { get; set; }
    }
}
