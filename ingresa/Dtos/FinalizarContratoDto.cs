namespace ingresa.Dtos
{
    public class FinalizarContratoDTO
    {

        public IFormFile File { get; set; }
        public DateOnly FechaFinContrato { get; set; }
        public int EmpID { get; set; }

    }
}
