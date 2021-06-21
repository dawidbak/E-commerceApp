using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using EcommerceApp.Application.Mapping;

namespace EcommerceApp.Application.ViewModels.Order
{
    public class OrderItemForCustomerOrderDetailsVM : IMapFrom<Domain.Models.OrderItem>
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public byte[] Image { get; set; }
        public string ImageToDisplay { get; set; }
        public string ProductName { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal UnitPrice { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal TotalPrice { get { return UnitPrice * Quantity; } }

        public void Mapping(Profile profile) => profile.CreateMap<Domain.Models.OrderItem, OrderItemForCustomerOrderDetailsVM>()
        .ForMember(x => x.ProductName, y => y.MapFrom(src => src.Product.Name))
        .ForMember(x => x.Image, y => y.MapFrom(src => src.Product.Image))
        .ForMember(x => x.UnitPrice, y => y.MapFrom(src => src.Product.UnitPrice));
    }
}
