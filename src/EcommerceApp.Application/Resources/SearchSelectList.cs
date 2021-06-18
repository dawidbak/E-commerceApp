using System;
using System.Collections.Generic;
using EcommerceApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcommerceApp.Application.Resources
{
    public class SearchSelectList : ISearchSelectList
    {
        public List<SelectListItem> CategorySelectList { get; } = new()
        {
            new SelectListItem { Value = "Id", Text = "Id" },
            new SelectListItem { Value = "Name", Text = "Name" },
        };

        public List<SelectListItem> ProductSelectList { get; } = new()
        {
            new SelectListItem { Value = "Id", Text = "Id" },
            new SelectListItem { Value = "Name", Text = "Name" },
            new SelectListItem { Value = "CategoryName", Text = "Category Name" },
            new SelectListItem { Value = "UnitPrice", Text = "Unit Price" },
            new SelectListItem { Value = "UnitsInStock", Text = "Units in Stock" },
        };

        public List<SelectListItem> EmployeeSelectList { get; } = new()
        {
            new SelectListItem { Value = "Id", Text = "Id" },
            new SelectListItem { Value = "Email", Text = "Email" },
            new SelectListItem { Value = "FirstName", Text = "First Name" },
            new SelectListItem { Value = "LastName", Text = "Last Name" },
            new SelectListItem { Value = "Position", Text = "Position" },
        };

        public List<SelectListItem> CustomerSelectList { get; } = new()
        {
            new SelectListItem { Value = "Id", Text = "Id" },
            new SelectListItem { Value = "Email", Text = "Email" },
            new SelectListItem { Value = "FirstName", Text = "First Name" },
            new SelectListItem { Value = "LastName", Text = "Last Name" },
            new SelectListItem { Value = "City", Text = "City" },
            new SelectListItem { Value = "PhoneNumber", Text = "Phone" },
            new SelectListItem { Value = "Address", Text = "Address" },
            new SelectListItem { Value = "PostalCode", Text = "Postal Code" },
        };

        public List<SelectListItem> OrderSelectList { get; } = new()
        {
            new SelectListItem { Value = "Id", Text = "Id" },
            new SelectListItem { Value = "CustomerId", Text = "Customer Id" },
            new SelectListItem { Value = "Email", Text = "Contact email" },
            new SelectListItem { Value = "Phone", Text = "Contact phone" },
            new SelectListItem { Value = "FirstName", Text = "First Name" },
            new SelectListItem { Value = "LastName", Text = "Last Name" },
            new SelectListItem { Value = "City", Text = "City" },
            new SelectListItem { Value = "PostalCode", Text = "Postal Code" },
            new SelectListItem { Value = "Address", Text = "Address" },
        };

        public List<SelectListItem> PageSizeSelectList { get; } = new()
        {
            new SelectListItem { Value = "5", Text = "5" },
            new SelectListItem { Value = "10", Text = "10" },
            new SelectListItem { Value = "20", Text = "20" },
            new SelectListItem { Value = "50", Text = "50" },
        };
    }
}
