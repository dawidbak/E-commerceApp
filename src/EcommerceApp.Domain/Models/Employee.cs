using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceApp.Domain.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [MinLength(2)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Position { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string AppUserId { get; set; }

        [ForeignKey("AppUserId")]
        public ApplicationUser AppUser { get; set; }
    }
}