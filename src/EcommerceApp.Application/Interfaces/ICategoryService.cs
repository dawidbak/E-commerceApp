using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceApp.Application.ViewModels.EmployeePanel;

namespace EcommerceApp.Application.Interfaces
{
    public interface ICategoryService
    {
        Task AddCategoryAsync(CategoryVM categoryVM);
        Task<CategoryVM> GetCategoryAsync(int categoryVMId);
        Task<ListCategoryForListVM> GetAllCategoriesAsync();
        Task<ListCategoryForListVM> GetAllPaginatedCategoriesAsync(int pageSize, int pageNumber);
        Task UpdateCategoryAsync(CategoryVM categoryVM);
        Task DeleteCategoryAsync(int categoryVMId);
    }
}
