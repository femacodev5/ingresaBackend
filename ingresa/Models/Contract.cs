using System.Diagnostics.Contracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ingresa.Models
{
    public class Contract 
    {
        public int ContractId { get; set; }

        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

        [DefaultValue(true)]
        public bool State { get; set; } = true;

        public int PersonId { get; set; }
        public Person Person{ get; set; }

        public decimal Salary { get; set; }
        public int Vacation { get; set; }


        public DateOnly? FechaConclucionContrato { get; set; }
        public ICollection<ContractFile> ContractFiles { get; set; }


    }
}
