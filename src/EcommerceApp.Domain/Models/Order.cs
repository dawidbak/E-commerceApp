using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceApp.Domain.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2")]
        public decimal Price { get; set; }

        [Required]
        [StringLength(50,MinimumLength = 2)]
        public string ShipFirstName { get; set; }

        [Required]
        [StringLength(50,MinimumLength = 2)]
        public string ShipLastName { get; set; }

        [Required]
        [EmailAddress]
        public string ShipContactEmail { get; set; }

        [Required]
        [Phone]
        public string ShipContactPhone { get; set; }

        [Required]
        [StringLength(50,MinimumLength = 2)]
        public string ShipCity { get; set; }

        [Required]
        [StringLength(10,MinimumLength = 5)]
        public string ShipPostalCode { get; set; }

        [Required]
        [StringLength(50,MinimumLength = 2)]
        public string ShipAddress { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public virtual Customer Customer { get; set; }
    }
}
