using System;
using System.Collections.Generic;
using EcommerceApp.Application.Mapping;

namespace EcommerceApp.Application.ViewModels.EmployeePanel
{
    public class ListProductForListVM : IMapFrom<PaginatedVM<ProductForListVM>>
    {
        public List<ProductForListVM> Products { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public void Mapping(AutoMapper.Profile profile) => profile.CreateMap<PaginatedVM<ProductForListVM>, ListProductForListVM>()
        .ForMember(x => x.Products, y => y.MapFrom(src => src.Items));
    }
}
