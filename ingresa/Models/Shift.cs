using System.ComponentModel.DataAnnotations;

namespace ingresa.Models
{

    
    public class Shift
    {

        public int ShiftId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Type{ get; set; }
        public ICollection<ShiftDetail> ShiftDetails { get; set; }


    }
}
