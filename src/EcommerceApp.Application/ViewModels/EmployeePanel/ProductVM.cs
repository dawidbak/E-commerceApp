using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using EcommerceApp.Application.Mapping;
using Microsoft.AspNetCore.Http;

namespace EcommerceApp.Application.ViewModels.EmployeePanel
{
    public class ProductVM : IMapFrom<Domain.Models.Product>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [Display(Name = "Unit Price")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "Units in Stock")]
        public int UnitsInStock { get; set; }

        [Display(Name = "Image")]
        public IFormFile ImageFormFile { get; set; }

        [Display(Name = "Image")]
        public string ImageUrl { get; set; }
        public byte[] Image { get; set; }


        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }

        public void Mapping(Profile profile) => profile.CreateMap<Domain.Models.Product, ProductVM>().ReverseMap();
    }
}
