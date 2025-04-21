using System.ComponentModel.DataAnnotations;

namespace Electronics.Models
{
    //Representa la estructura de Rol.
    public class Rol
    {
        [Key]
        public int IdRol { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }
    }
}
