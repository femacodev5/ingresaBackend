using Microsoft.Build.ObjectModelRemoting;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace ingresa.Models
{
    public class Persona
    {
        public int PersonaId { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string Apellido { get; set; }

        public string Correo{ get; set; }


        [Required]
        public string NumeroDocumento { get; set; }

        //contract

        public ICollection<Contrato> Contractos { get; set; }


        //cluster 
        public int GrupoId { get; set; }
        public Grupo Grupo { get; set; }

        public int? TurnoId { get; set; }
        public Turno? Turno { get; set; }

        public ICollection<Marcacion> Marcaciones { get; set; }
        public ICollection<Asistencia> Asistencias { get; set; }


    }
}
