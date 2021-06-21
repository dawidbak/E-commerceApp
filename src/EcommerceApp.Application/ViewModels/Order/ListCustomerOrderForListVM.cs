using System;
using System.Collections.Generic;
using AutoMapper;
using EcommerceApp.Application.Mapping;

namespace EcommerceApp.Application.ViewModels.Order
{
    public class ListCustomerOrderForListVM : IMapFrom<PaginatedVM<CustomerOrderForListVM>>
    {
        public List<CustomerOrderForListVM> Orders { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }

        public void Mapping(Profile profile) => profile.CreateMap<PaginatedVM<CustomerOrderForListVM>, ListCustomerOrderForListVM>()
        .ForMember(x => x.Orders, y => y.MapFrom(src => src.Items));
    }
}
