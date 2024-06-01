namespace ingresa.Responses
{
    public class ContractReport
    {
        
    public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DocumentNumber { get; set; }
        public string FileName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime FechaActual { get; set; }
        public int NumContratos { get; set; }
        public string EstadoContrato { get; set; }
    
}
}
