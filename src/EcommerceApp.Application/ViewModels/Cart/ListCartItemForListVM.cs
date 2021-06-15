using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Application.ViewModels.Cart
{
    public class ListCartItemForListVM
    {
        public List<CartItemForListVM> CartItems { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal TotalPrice
        {
            get
            {
                var result = 0m;
                {
                    foreach (var cartItem in CartItems)
                    {
                        result += cartItem.TotalCartItemPrice;
                    }
                }
                return result;
            }
        }
    }
}
