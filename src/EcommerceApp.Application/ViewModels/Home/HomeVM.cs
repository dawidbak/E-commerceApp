using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceApp.Application.ViewModels.EmployeePanel;

namespace EcommerceApp.Application.ViewModels.Home
{
    public class HomeVM
    {
        public List<CategoryVM> Categories = new List<CategoryVM>();
        public List<ProductVM> Products = new List<ProductVM>();
    }
}
