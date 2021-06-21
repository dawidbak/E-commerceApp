using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using EcommerceApp.Application.Mapping;

namespace EcommerceApp.Application.ViewModels.AdminPanel
{
    public class CustomerDetailsVM : IMapFrom<Domain.Models.Customer>
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Phone")]
        public string PhoneNumber { get; set; }
        public List<OrderForCustomerDetailsVM> Orders { get; set; }
        public List<CartItemForCustomerDetailsVM> CartItems { get; set; }

        public void Mapping(Profile profile) => profile.CreateMap<Domain.Models.Customer, CustomerDetailsVM>()
        .ForMember(x => x.Email, y => y.MapFrom(src => src.AppUser.Email))
        .ForMember(x => x.PhoneNumber, y => y.MapFrom(src => src.AppUser.PhoneNumber));
    }
}
