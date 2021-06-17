using System;
using System.Collections.Generic;

namespace EcommerceApp.Application.ViewModels.AdminPanel
{
    public class ListEmployeeForListVM
    {
        public List<EmployeeForListVM> Employees { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
