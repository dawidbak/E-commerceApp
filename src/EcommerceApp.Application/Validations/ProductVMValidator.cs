using System;
using EcommerceApp.Application.ViewModels.EmployeePanel;
using FluentValidation;

namespace EcommerceApp.Application.Validations
{
    public class ProductVMValidator : AbstractValidator<ProductVM>
    {
        public ProductVMValidator()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Description).MaximumLength(200);
            RuleFor(x => x.UnitPrice).NotEmpty().ScalePrecision(2, 18);
            RuleFor(x => x.UnitsInStock).NotNull();
            RuleFor(x => x.ImageFormFile).SetValidator(new FileValidator());
        }
    }
}
