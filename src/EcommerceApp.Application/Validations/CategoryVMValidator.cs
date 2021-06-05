using System;
using EcommerceApp.Application.ViewModels.EmployeePanel;
using FluentValidation;

namespace EcommerceApp.Application.Validations
{
    public class CategoryVMValidator : AbstractValidator<CategoryVM>
    {
        public CategoryVMValidator()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Description).MaximumLength(200);
            RuleFor(x => x.ImageFormFile).SetValidator(new FileValidator());
        }
    }
}
