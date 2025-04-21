using System.ComponentModel.DataAnnotations;

namespace Electronics.Models
{
    //Representa la estructura de sucursal.
    public class Sucursal
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Direccion { get; set; }

        [Required]
        public double Latitud { get; set; }

        [Required]
        public double Longitud { get; set; }
    }
}
//:)