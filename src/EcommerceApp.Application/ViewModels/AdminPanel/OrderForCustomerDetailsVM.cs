using System;
using System.ComponentModel.DataAnnotations;
using EcommerceApp.Application.Mapping;

namespace EcommerceApp.Application.ViewModels.AdminPanel
{
    public class OrderForCustomerDetailsVM : IMapFrom<Domain.Models.Order>
    {
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Price { get; set; }

        [Display(Name = "Order date")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime OrderDate { get; set; }
    }
}
