using ingresa.Models;

namespace ingresa.Dtos
{
    public class ShiftResponse
    {
        public int TurnoId { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public ICollection<DetalleTurno> DetalleTurnos { get; set; }
        public int TotalMinutosJornadaNeto { get; set; }

    }
}
