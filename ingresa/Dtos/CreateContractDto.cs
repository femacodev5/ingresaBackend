using System.ComponentModel.DataAnnotations;

namespace ingresa.Dtos
{
    public class CreateContractDto
    {
        public decimal Salario { get; set; }
        public int Vacaciones { get; set; }
        public DateOnly FechaFin { get; set; }
        public DateOnly FechaInicio { get; set; }
        public int EmpID { get; set; }
        public IFormFile? File { get; set; }
    }

}
