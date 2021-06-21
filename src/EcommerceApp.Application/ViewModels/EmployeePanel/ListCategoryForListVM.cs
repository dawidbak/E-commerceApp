using System;
using System.Collections.Generic;
using AutoMapper;
using EcommerceApp.Application.Mapping;

namespace EcommerceApp.Application.ViewModels.EmployeePanel
{
    public class ListCategoryForListVM : IMapFrom<PaginatedVM<CategoryForListVM>>
    {
        public List<CategoryForListVM> Categories { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public void Mapping(Profile profile) => profile.CreateMap<PaginatedVM<CategoryForListVM>, ListCategoryForListVM>()
        .ForMember(x => x.Categories, y => y.MapFrom(src => src.Items));
    }
}
