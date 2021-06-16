using System.Data;
using System;
using EcommerceApp.Application.ViewModels.Order;
using FluentValidation;

namespace EcommerceApp.Application.Validations
{
    public class OrderCheckoutVMValidator : AbstractValidator<OrderCheckoutVM>
    {
        public OrderCheckoutVMValidator()
        {
            RuleFor(x => x.CartItems).NotNull();
            RuleFor(x => x.CustomerId).NotNull();
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50).MinimumLength(2);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(50).MinimumLength(2);
            RuleFor(x => x.TotalPrice).NotNull();
            RuleFor(x => x.Email).EmailAddress().NotEmpty().MaximumLength(256);
            RuleFor(x => x.City).NotEmpty().MaximumLength(50).MinimumLength(2);
            RuleFor(x => x.PostalCode).NotEmpty().MaximumLength(10).MinimumLength(5);
            RuleFor(x => x.Address).NotEmpty().MaximumLength(50).MinimumLength(2);
        }
    }
}
