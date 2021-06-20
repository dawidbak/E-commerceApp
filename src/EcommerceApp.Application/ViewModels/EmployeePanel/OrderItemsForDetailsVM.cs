using System;
using AutoMapper;
using EcommerceApp.Application.Mapping;

namespace EcommerceApp.Application.ViewModels.EmployeePanel
{
    public class OrderItemsForDetailsVM : IMapFrom<Domain.Models.OrderItem>
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }

        public void Mapping(Profile profile) => profile.CreateMap<Domain.Models.OrderItem, OrderItemsForDetailsVM>()
        .ForMember(x => x.ProductName, y => y.MapFrom(src => src.Product.Name));
    }
}
