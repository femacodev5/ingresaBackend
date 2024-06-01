using System.ComponentModel.DataAnnotations;

namespace ingresa.Dtos
{
    public class CreateContractDto
    {
        public decimal Salary { get; set; }
        public int  Vacation { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int PersonId { get; set; }
        public IFormFile? File { get; set; }
    }

}
