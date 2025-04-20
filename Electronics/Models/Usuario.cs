using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Electronics.Models
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        [Display(Name = "Primer Apellido")]
        public string Primer_Apellido { get; set; }

        [Required]
        [Display(Name = "Segundo Apellido")]
        public string Segundo_Apellido { get; set; }

        [Required]
        [EmailAddress]
        public string Correo { get; set; }

        [Required]
        public string Contrasena { get; set; }

        // Rol (1=Admin, 2=Cliente)
        public int IdRol { get; set; }

        public int? IdFotoPerfil { get; set; }

        [ForeignKey(nameof(IdFotoPerfil))]
        public virtual Imagenes FotoPerfil { get; set; }

        [NotMapped]
        public string FotoPerfilRuta
            => FotoPerfil != null
               ? FotoPerfil.Ruta
               : "/imagenes/perfil.png";
    }
}
