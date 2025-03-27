using System.ComponentModel.DataAnnotations;

namespace Electronics.Models
{
    public class Imagenes
    {
        [Key]
        public int Id { get; set; }


        public string Nombre { get; set; }


        public string Ruta { get; set; }
    }
}

