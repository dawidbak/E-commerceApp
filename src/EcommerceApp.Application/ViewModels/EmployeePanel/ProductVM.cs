using System.Data;
using System;
using EcommerceApp.Application.Mapping;
using AutoMapper;
using FluentValidation;
using System.Collections.Generic;

namespace EcommerceApp.Application.ViewModels.EmployeePanel
{
    public class ProductVM : IMapFrom<Domain.Models.Product>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
        public byte[] Picture { get; set; }
        public string CategoryName { get; set; }

        public void Mapping(Profile profile) => profile.CreateMap<Domain.Models.Product, ProductVM>().ReverseMap();
    }

    public class ProductValidator : AbstractValidator<ProductVM>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Description).MaximumLength(200);
            RuleFor(x => x.UnitPrice).NotEmpty().ScalePrecision(2,18);
            RuleFor(x => x.UnitsInStock).NotNull();
        }
    }
}
