using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using EcommerceApp.Application.Mapping;

namespace EcommerceApp.Application.ViewModels.EmployeePanel
{
    public class OrderForListVM : IMapFrom<Domain.Models.Order>
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime OrderDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Price { get; set; }
        public string ShipFirstName { get; set; }
        public string ShipLastName { get; set; }
        public string ShipContactEmail { get; set; }
        public string ShipContactPhone { get; set; }
        public string ShipCity { get; set; }
        public string ShipPostalCode { get; set; }
        public string ShipAddress { get; set; }

        public void Mapping(Profile profile) => profile.CreateMap<Domain.Models.Order, OrderForListVM>();
    }
}
