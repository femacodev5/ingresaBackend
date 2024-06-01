using System.ComponentModel.DataAnnotations;

namespace ingresa.Models
{

    public enum Role
    {
        User,
        Administrator,
        Editor,
        Guest 
    }
    public class User
    {

        public int UserId { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; }

        [Required]
        public Role Role { get; set; }


        public string Email { get; set; }




        public int PersonId { get; set; }
        public Person Person{ get; set; }

    }
}
