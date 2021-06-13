using System;
using AutoMapper;
using EcommerceApp.Application.Mapping;

namespace EcommerceApp.Application.ViewModels.Cart
{
    public class CartItemForListVM : IMapFrom<Domain.Models.CartItem>
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

        public void Mapping(Profile profile) => profile.CreateMap<Domain.Models.CartItem, CartItemForListVM>();
    }
}
