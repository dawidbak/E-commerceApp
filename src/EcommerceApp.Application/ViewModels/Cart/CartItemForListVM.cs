using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.Mapping;

namespace EcommerceApp.Application.ViewModels.Cart
{
    public class CartItemForListVM : IMapFrom<Domain.Models.CartItem>
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
        public byte[] Image { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal UnitPrice { get; set; }
        public int UnitsInStock { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal TotalCartItemPrice { get { return UnitsInStock >= Quantity ? Quantity * UnitPrice : 0; } }

        public void Mapping(Profile profile) => profile.CreateMap<Domain.Models.CartItem, CartItemForListVM>()
        .ForMember(x => x.Name, y => y.MapFrom(src => src.Product.Name))
        .ForMember(x => x.UnitPrice, y => y.MapFrom(src => src.Product.UnitPrice))
        .ForMember(x => x.Image, y => y.MapFrom(src => src.Product.Image))
        .ForMember(x => x.UnitsInStock, y => y.MapFrom(src => src.Product.UnitsInStock));
    }
}
