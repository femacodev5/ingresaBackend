using System.Diagnostics.Contracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ingresa.Models
{
    public class Contrato
    {
        public int ContratoId { get; set; }

        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }

        [DefaultValue(true)]
        public bool Estado { get; set; } = true;

        public int PersonaId { get; set; }
        public Persona Persona{ get; set; }

        public decimal Salario { get; set; }
        public int Vacaciones { get; set; }


        public DateOnly? FechaConclucionContrato { get; set; }
        public ICollection<ArchivoContrato> ArchivosContratos { get; set; }


    }
}
