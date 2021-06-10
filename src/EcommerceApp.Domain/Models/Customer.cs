using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceApp.Domain.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string City { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 5)]
        public string PostalCode { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Address { get; set; }

        public string AppUserId { get; set; }

        [ForeignKey("AppUserId")]
        public virtual ApplicationUser AppUser { get; set; }

        public virtual Cart Cart { get; set; }
    }
}
