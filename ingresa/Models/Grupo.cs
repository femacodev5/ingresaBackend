using System.ComponentModel.DataAnnotations;

namespace ingresa.Models
{
    public class Grupo
    {
        public int GrupoId { get; set; }
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }


        public string Descripcion { get; set; }



        public ICollection<Persona> Persona { get; set; }
    }
}
