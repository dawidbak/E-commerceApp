using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceApp.Domain.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2")]
        public decimal UnitPrice { get; set; }

        [Required]
        public int UnitsInStock { get; set; }

        public byte[] Image { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        [InverseProperty("Products")]
        public virtual Category Category { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }
    }
}
