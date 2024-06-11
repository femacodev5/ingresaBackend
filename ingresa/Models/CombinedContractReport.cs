using ingresa.Responses;

namespace ingresa.Models
{
    public class CombinedContractReport
    {
        public int EmpID { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string FirstName { get; set; }
        public string Code { get; set; }
        public string JobTitle { get; set; }
        public List<ContractReport> Contracts { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public DateTime FechaActual { get; set; }
        public int NumContratos { get; set; }
        public string EstadoContrato { get; set; }

    }
}
