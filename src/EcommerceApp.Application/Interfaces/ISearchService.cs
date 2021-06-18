using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceApp.Application.ViewModels.AdminPanel;
using EcommerceApp.Application.ViewModels.EmployeePanel;

namespace EcommerceApp.Application.Interfaces
{
    public interface ISearchService
    {
        Task<ListCategoryForListVM> SearchSelectedCategoriesAsync(string selectedValue, string searchString, int pageSize, int pageNumber);
        Task<ListProductForListVM> SearchSelectedProductsAsync(string selectedValue, string searchString, int pageSize, int pageNumber);
        Task<ListEmployeeForListVM> SearchSelectedEmployeesAsync(string selectedValue, string searchString, int pageSize, int pageNumber);
        Task<ListCustomerVM> SearchSelectedCustomersAsync(string selectedValue, string searchString, int pageSize, int pageNumber);
        Task<ListOrderForListVM> SearchSelectedOrdersAsync(string selectedValue, string searchString, int pageSize, int pageNumber);
    }
}
