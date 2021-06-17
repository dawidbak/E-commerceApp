using System;
using System.Collections.Generic;

namespace EcommerceApp.Application.ViewModels.AdminPanel
{
    public class ListCustomerVM
    {
        public List<CustomerVM> Customers { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
