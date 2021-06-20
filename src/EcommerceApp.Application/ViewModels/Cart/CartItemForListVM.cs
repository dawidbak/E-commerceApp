using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
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

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal UnitPrice { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal TotalCartItemPrice
        {
            get => Quantity * UnitPrice;
        }

        public void Mapping(Profile profile) => profile.CreateMap<Domain.Models.CartItem, CartItemForListVM>()
        .ForMember(x => x.Name, y => y.MapFrom(src => src.Product.Name))
        .ForMember(x => x.ImageUrl, y => y.MapFrom(src => string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(src.Product.Image))))
        .ForMember(x => x.UnitPrice, y => y.MapFrom(src => src.Product.UnitPrice));
    }
}
