using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using EcommerceApp.Application.Mapping;

namespace EcommerceApp.Application.ViewModels.Cart
{
    public class ListCartItemForListVM : IMapFrom<Domain.Models.Cart>
    {
        public int CustomerId { get; set; }
        public List<CartItemForListVM> CartItems { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal TotalPrice { get; set; }

        public void Mapping(Profile profile) => profile.CreateMap<Domain.Models.Cart, ListCartItemForListVM>()
        .ForMember(x => x.CartItems, y => y.MapFrom(src => src.CartItems));
    }
}
