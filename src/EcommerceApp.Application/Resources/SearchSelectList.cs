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
            new SelectListItem { Value = "CategoryName", Text="Category Name"},
            new SelectListItem { Value = "UnitPrice", Text="Unit Price"},
            new SelectListItem { Value = "UnitsInStock", Text="Units in Stock"},
        };
    }
}
