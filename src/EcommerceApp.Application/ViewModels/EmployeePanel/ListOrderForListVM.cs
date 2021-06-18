using System;
using System.Collections.Generic;

namespace EcommerceApp.Application.ViewModels.EmployeePanel
{
    public class ListOrderForListVM
    {
        public List<OrderForListVM> Orders { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
