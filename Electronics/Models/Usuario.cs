using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace Electronics.Models
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(50)]
        public string Primer_Apellido { get; set; }

        [StringLength(50)]
        public string Segundo_Apellido { get; set; }

        [Required]
        [StringLength(100)]
        public string Correo { get; set; }

        [Required]
        [StringLength(100)]
        public string Contrasena { get; set; }

        [Required]
        public int IdRol { get; set; }

        public int? IdFotoPerfil { get; set; }

        // Relaciones (si las tienes)
        [ForeignKey("IdRol")]
        public virtual Rol Rol { get; set; }

        [ForeignKey("IdFotoPerfil")]
        public virtual Imagenes FotoPerfil { get; set; }
    }
}
