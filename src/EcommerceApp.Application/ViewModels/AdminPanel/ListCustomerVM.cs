using System;
using System.Collections.Generic;
using AutoMapper;
using EcommerceApp.Application.Mapping;

namespace EcommerceApp.Application.ViewModels.AdminPanel
{
    public class ListCustomerVM : IMapFrom<PaginatedVM<CustomerVM>>
    {
        public List<CustomerVM> Customers { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public void Mapping(Profile profile) => profile.CreateMap<PaginatedVM<CustomerVM>, ListCustomerVM>()
        .ForMember(x => x.Customers, y => y.MapFrom(src => src.Items));
    }
}
