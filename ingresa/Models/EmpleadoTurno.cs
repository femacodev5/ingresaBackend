namespace ingresa.Models
{
    public class EmpleadoTurno
    {

        public int EmpleadoTurnoId { get; set; }
        public int EmpId { get; set; }
        public int TurnoId {  get; set; }
        public bool Estado { get; set; } = true;


    }
}
