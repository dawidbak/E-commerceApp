using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using EcommerceApp.Application.Mapping;

namespace EcommerceApp.Application.ViewModels.AdminPanel
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
}
