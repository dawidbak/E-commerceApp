using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EcommerceApp.Application.ViewModels.Cart;

namespace EcommerceApp.Application.ViewModels.Order
{
    public class OrderCheckoutVM
    {
        public List<CartItemForListVM> CartItems { get; set; }
        public int CartId { get; set; }
        public int CustomerId { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }


        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal TotalPrice { get; set; }
        public string Email { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
    }
}
