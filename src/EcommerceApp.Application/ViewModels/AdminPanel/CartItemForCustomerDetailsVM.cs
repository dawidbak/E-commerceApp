using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using EcommerceApp.Application.Mapping;

namespace EcommerceApp.Application.ViewModels.AdminPanel
{
    public class CartItemForCustomerDetailsVM : IMapFrom<Domain.Models.CartItem>
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
        public byte[] Image { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal UnitPrice { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal TotalCartItemPrice
        {
            get => Quantity * UnitPrice;
        }

        public void Mapping(Profile profile) => profile.CreateMap<Domain.Models.CartItem, CartItemForCustomerDetailsVM>()
        .ForMember(x => x.ProductName, y => y.MapFrom(src => src.Product.Name))
        .ForMember(x => x.UnitPrice, y => y.MapFrom(src => src.Product.UnitPrice))
        .ForMember(x => x.Image, y => y.MapFrom(src => src.Product.Image));
    }
}
