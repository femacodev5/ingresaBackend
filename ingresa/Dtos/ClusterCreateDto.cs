using System.ComponentModel.DataAnnotations;

namespace ingresa.Dtos
{
    public class ClusterCreateDto
    {
        [Required]
        [StringLength(100)]
        public string ClusterName { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
