using System.ComponentModel.DataAnnotations;
using System;
using AutoMapper;
using EcommerceApp.Application.Mapping;

namespace EcommerceApp.Application.ViewModels.Cart
{
    public class CartItemForListVM : IMapFrom<Domain.Models.Product>
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal UnitPrice { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal TotalPrice { get; set; }

        public void Mapping(Profile profile) => profile.CreateMap<Domain.Models.Product, CartItemForListVM>();
    }
}
