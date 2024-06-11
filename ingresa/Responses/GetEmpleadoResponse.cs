namespace ingresa.Responses
{
    public class GetEmpleadoResponse
    {
        public int EmpID { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string FirstName { get; set; }
        public string Code { get; set; }
        public string JobTitle { get; set; }
        public string TurnoNombre { get; set; }
        public int? TurnoId { get; set; }
    }
}
