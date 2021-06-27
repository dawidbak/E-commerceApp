using System;
using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Domain.Interfaces
{
    public interface ICategoryRepository
    {
        Task AddCategoryAsync(Category category);
        Task<Category> GetCategoryAsync(int categoryId);
        IQueryable<Category> GetAllCategories();
        Task UpdateCategoryAsync(Category Category);
        Task DeleteCategoryAsync(int categoryId);
    }
}
