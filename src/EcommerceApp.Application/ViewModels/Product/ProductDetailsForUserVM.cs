using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using EcommerceApp.Application.Mapping;

namespace EcommerceApp.Application.ViewModels.Product
{
    public class ProductDetailsForUserVM : IMapFrom<Domain.Models.Product>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        public void Mapping(Profile profile) => profile.CreateMap<Domain.Models.Product, ProductDetailsForUserVM>();
    }
}
