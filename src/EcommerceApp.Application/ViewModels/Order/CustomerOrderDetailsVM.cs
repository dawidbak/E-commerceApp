using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using EcommerceApp.Application.Mapping;

namespace EcommerceApp.Application.ViewModels.Order
{
    public class CustomerOrderDetailsVM : IMapFrom<Domain.Models.Order>
    {
        [Display(Name = "Order Id")]
        public int Id { get; set; }

        [Display(Name = "Order Date")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime OrderDate { get; set; }

        [Display(Name = "Price")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Price { get; set; }

        [Display(Name = "First Name")]
        public string ShipFirstName { get; set; }

        [Display(Name = "Last Name")]
        public string ShipLastName { get; set; }

        [Display(Name = "Contact email")]
        public string ShipContactEmail { get; set; }

        [Display(Name = "Contact phone")]
        public string ShipContactPhone { get; set; }

        [Display(Name = "City")]
        public string ShipCity { get; set; }

        [Display(Name = "Postal Code")]
        public string ShipPostalCode { get; set; }

        [Display(Name = "Address")]
        public string ShipAddress { get; set; }
        public List<OrderItemForCustomerOrderDetailsVM> OrderItems { get; set; }

        public void Mapping(Profile profile) => profile.CreateMap<Domain.Models.Order, CustomerOrderDetailsVM>()
        .ForMember(x => x.OrderItems, y => y.MapFrom(src => src.OrderItems));
    }
}
