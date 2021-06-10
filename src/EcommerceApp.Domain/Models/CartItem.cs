using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceApp.Domain.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Product Product { get; set; }
        
        [Required]
        public int Quantity { get; set; }

        public int CartId { get; set; }
        
        [ForeignKey(nameof(CartId))]
        public virtual Cart Cart { get; set; }
    }
}
