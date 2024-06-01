using Microsoft.Build.ObjectModelRemoting;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace ingresa.Models
{
    public class Person
    {
        public int PersonId { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        public string Email{ get; set; }


        [Required]
        public string DocumentNumber { get; set; }

        //contract

        public ICollection<Contract> Contracts { get; set; }

       

        //cluster 
        public int ClusterId { get; set; }
        public Cluster Cluster { get; set; }

    }
}
