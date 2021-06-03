using System;
using EcommerceApp.Application.Mapping;
using AutoMapper;

namespace EcommerceApp.Application.ViewModels.EmployeePanel
{
    public class CategoriesVM : IMapFrom<Domain.Models.Category>
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }

        public void Mapping(Profile profile) => profile.CreateMap<Domain.Models.Category,CategoriesVM>();
    }
}
