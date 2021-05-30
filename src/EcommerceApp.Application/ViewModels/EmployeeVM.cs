using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using EcommerceApp.Application.Mapping;
using FluentValidation;

namespace EcommerceApp.Application.ViewModels
{
    public class EmployeeVM : IMapFrom<Domain.Models.Employee>
    {
        public int Id { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }
        
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }

        public void Mapping(Profile profile) => profile.CreateMap<Domain.Models.Employee, EmployeeVM>().ReverseMap();

    }
        public class EmployeeValidator : AbstractValidator<EmployeeVM>
        {
            public EmployeeValidator()
            {
                RuleFor(x => x.Id).NotNull().WithMessage("You must provide a valid {Id}");
                RuleFor(x => x.FirstName).NotEmpty().MinimumLength(2).MaximumLength(50)
                .WithMessage("The {FirstName} must be at least {MinimumLength} and at max {MaximumLength} characters long.");
                RuleFor(x => x.LastName).NotEmpty().MinimumLength(2).MaximumLength(50)
                .WithMessage("The {LastName} must be at least {MinimumLength} and at max {MaximumLength} characters long.");
                RuleFor(x => x.Position).NotEmpty().MinimumLength(2).MaximumLength(50)
                .WithMessage("The {LastName} must be at least {MinimumLength} and at max {MaximumLength} characters long.");
                RuleFor(x => x.Email).EmailAddress().NotEmpty();
                
            }
        }
}
