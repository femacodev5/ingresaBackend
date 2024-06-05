using System.ComponentModel.DataAnnotations;

namespace ingresa.Models
{

    
    public class Turno
    {

        public int TurnoId { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Tipo{ get; set; }
        public ICollection<DetalleTurno> DetalleTurnos { get; set; }


        public ICollection<Persona> Personas{ get; set; }


    }
}
