using System.ComponentModel.DataAnnotations;

namespace ingresa.Models
{

    public enum Role
    {
        User,
        Administrator,
        Editor,
        Guest 
    }
    public class Usuario
    {

        public int UsuarioId { get; set; }

        [Required]
        public string NombreUsuario{ get; set; }

        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string Clave { get; set; }

        [Required]
        public Role Rol { get; set; }

        public string Email { get; set; }

        public int PersonaId { get; set; }
        public Persona Persona{ get; set; }

    }
}
