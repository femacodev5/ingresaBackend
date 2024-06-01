using System.ComponentModel.DataAnnotations;

namespace ingresa.Models
{
    public class Cluster
    {
        public int ClusterId { get; set; }
        [Required]
        [StringLength(100)]
        public string ClusterName { get; set; }


        public string Description { get; set; }



        public ICollection<Person> Person { get; set; }
    }
}
