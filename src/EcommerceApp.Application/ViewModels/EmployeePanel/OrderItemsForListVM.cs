using System;
using AutoMapper;
using EcommerceApp.Application.Mapping;

namespace EcommerceApp.Application.ViewModels.EmployeePanel
{
    public class OrderItemsForListVM : IMapFrom<Domain.Models.OrderItem>
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Name { get; set; }

        public void Mapping(Profile profile) => profile.CreateMap<Domain.Models.OrderItem, OrderItemsForListVM>();
    }
}
