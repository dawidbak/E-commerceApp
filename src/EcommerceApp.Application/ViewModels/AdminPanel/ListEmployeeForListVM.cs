using System;
using System.Collections.Generic;
using AutoMapper;
using EcommerceApp.Application.Mapping;

namespace EcommerceApp.Application.ViewModels.AdminPanel
{
    public class ListEmployeeForListVM : IMapFrom<PaginatedVM<EmployeeForListVM>>
    {
        public List<EmployeeForListVM> Employees { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public void Mapping(Profile profile) => profile.CreateMap<PaginatedVM<EmployeeForListVM>, ListEmployeeForListVM>()
        .ForMember(x => x.Employees, y => y.MapFrom(src => src.Items));
    }
}
