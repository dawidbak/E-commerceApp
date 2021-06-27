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

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Position")]
        public string Position { get; set; }

        public void Mapping(Profile profile) => profile.CreateMap<Domain.Models.Employee, EmployeeVM>()
        .ForMember(x => x.Email, y => y.MapFrom(src => src.AppUser.Email))
        .ReverseMap()
        .ForPath(x => x.AppUser.Email, y => y.Ignore());

    }
}
