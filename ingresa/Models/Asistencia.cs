using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace ingresa.Models
{
    public class Asistencia
    {
        public int AsistenciaId { get; set; }
        public int InicioMarcacionId { get; set; }
        public int FinMarcacionId { get; set; }


        public int InicioDescansoId { get; set; }
        public int FinDescansoId { get; set; }
        public string EstadoDescanso{ get; set; }


        public int MinutosAsistencia { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime FechaCreacion { get; set; }
        public DateOnly Fecha { get; set; }

        public string Estado { get; set; }
        public int PersonaId { get; set; }
        public Persona Persona{ get; set; }
    }
}
