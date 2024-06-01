using ingresa.Models;

namespace ingresa.Dtos
{
    public class ShiftResponse
    {
        public int ShiftId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public ICollection<ShiftDetail> ShiftDetails{ get; set; }
        public int TotalMinutosJornadaNeto { get; set; }

    }
}
