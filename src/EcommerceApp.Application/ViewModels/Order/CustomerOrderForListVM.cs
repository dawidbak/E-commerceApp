using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using EcommerceApp.Application.Mapping;

namespace EcommerceApp.Application.ViewModels.Order
{
    public class CustomerOrderForListVM : IMapFrom<Domain.Models.Order>
    {
        [Display(Name = "Order Id")]
        public int Id { get; set; }

        [Display(Name = "Order Date")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime OrderDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Price { get; set; }

        public void Mapping(Profile profile) => profile.CreateMap<Domain.Models.Order, CustomerOrderForListVM>();
    }
}
