using System;
using System.Collections.Generic;

namespace EcommerceApp.Application.ViewModels.EmployeePanel
{
    public class ListProductForListVM
    {
        public List<ProductForListVM> Products { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
