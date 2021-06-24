using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using EcommerceApp.Application.Mapping;

namespace EcommerceApp.Application.ViewModels.EmployeePanel
{
    public class OrderDetailsVM : IMapFrom<Domain.Models.Order>
    {
        [Display(Name = "Order Id")]
        public int Id { get; set; }

        [Display(Name = "Customer Id")]
        public int CustomerId { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "Price")]
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
        public List<OrderItemsForDetailsVM> OrderItems { get; set; }

        public void Mapping(Profile profile) => profile.CreateMap<Domain.Models.Order, OrderDetailsVM>()
        .ForMember(x => x.OrderItems, y => y.MapFrom(src => src.OrderItems));
    }
}
