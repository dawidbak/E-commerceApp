using System;
using AutoMapper;
using EcommerceApp.Application.Mapping;
using FluentValidation;

namespace EcommerceApp.Application.ViewModels.EmployeePanel
{
    public class CategoryVM : IMapFrom<Domain.Models.Category>
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Picture { get; set;}

        public void Mapping(Profile profile) => profile.CreateMap<Domain.Models.Category,CategoryVM>().ReverseMap();

    }
        public class CategoryValidator : AbstractValidator<CategoryVM>
        {
            public CategoryValidator()
            {
                RuleFor(x => x.CategoryId).NotNull();
                RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
                RuleFor(x => x.Description).MaximumLength(200);
            }
        }
}
