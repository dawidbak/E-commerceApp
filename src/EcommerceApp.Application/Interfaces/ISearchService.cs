using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceApp.Application.ViewModels.AdminPanel;
using EcommerceApp.Application.ViewModels.EmployeePanel;

namespace EcommerceApp.Application.Interfaces
{
    public interface ISearchService
    {
        Task<ListCategoryForListVM> SearchSelectedCategoryAsync(string selectedValue, string searchString, int pageSize, int pageNumber);
        Task<ListProductForListVM> SearchSelectedProductAsync(string selectedValue, string searchString, int pageSize, int pageNumber);
        Task<ListEmployeeForListVM> SearchSelectedEmployeeAsync(string selectedValue, string searchString, int pageSize, int pageNumber);
        Task<ListCustomerVM> SearchSelectedCustomerAsync(string selectedValue, string searchString, int pageSize, int pageNumber);
    }
}
