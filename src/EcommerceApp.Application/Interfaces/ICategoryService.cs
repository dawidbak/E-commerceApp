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
        Task<List<CategoryVM>> GetAllCategoriesAsync();
        Task UpdateCategoryAsync(CategoryVM categoryVM);
        Task DeleteCategoryAsync(int categoryVMId);
    }
}
