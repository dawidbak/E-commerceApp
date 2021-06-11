using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceApp.Application.ViewModels.AdminPanel;
using EcommerceApp.Application.ViewModels.EmployeePanel;

namespace EcommerceApp.Application.Interfaces
{
    public interface ISearchService
    {
        Task<List<CategoryVM>> SearchSelectedCategoryAsync(string selectedValue, string searchString);
        Task<List<ProductVM>> SearchSelectedProductAsync(string selectedValue, string searchString);
        Task<List<EmployeeVM>> SearchSelectedEmployeeAsync(string selectedValue, string searchString);
        Task<List<CustomerVM>> SearchSelectedCustomerAsync(string selectedValue, string searchString);
    }
}
