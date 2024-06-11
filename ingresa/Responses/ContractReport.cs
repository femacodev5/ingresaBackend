namespace ingresa.Responses
{
    public class ContractReport
    {
        
        public int PersonaId { get; set; }
        public string Nombre{ get; set; }
        public string Apellido { get; set; }
        public int EmpID { get; set; }
        public string NumeroDocumento{ get; set; }
        public string FileName { get; set; }
        public DateTime? FechaInicio{ get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime FechaActual { get; set; }
        public int NumContratos { get; set; }
        public string EstadoContrato { get; set; }
    
}
}
