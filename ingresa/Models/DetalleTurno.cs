namespace ingresa.Models
{
    public class DetalleTurno
    {
        public int DetalleTurnoId { get; set; }
      
        public int DiaSemana { get; set; } // 0 para domingo, 1 para lunes, etc.
        public TimeSpan InicioMarcacionEntrada{ get; set; }
        public TimeSpan FinMarcacionEntrada { get; set; }
        public TimeSpan HoraEntrada { get; set; }


        public TimeSpan InicioMarcacionDescanso { get; set; }
        public TimeSpan FinMarcacionDescanso { get; set; }
        public int MinutosDescanso { get; set; }

        public bool HabilitarDescanso { get; set; }

        public TimeSpan InicioMarcacionSalida { get; set; }
        public TimeSpan HoraSalida { get; set; }
        public TimeSpan FinMarcacionSalida { get; set; }
        public int MinutosJornada { get; set; }
        public int MinutosJornadaNeto { get; set; }


        public int TurnoId { get; set; }
        public Turno Turno{ get; set; }
    }
}
