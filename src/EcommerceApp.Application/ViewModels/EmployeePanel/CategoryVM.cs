using System;
using AutoMapper;
using EcommerceApp.Application.Mapping;
using Microsoft.AspNetCore.Http;

namespace EcommerceApp.Application.ViewModels.EmployeePanel
{
    public class CategoryVM : IMapFrom<Domain.Models.Category>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile ImageFormFile { get; set; }
        public string ImageUrl { get; set; }

        public void Mapping(Profile profile) => profile.CreateMap<Domain.Models.Category, CategoryVM>().ReverseMap();
    }
}
