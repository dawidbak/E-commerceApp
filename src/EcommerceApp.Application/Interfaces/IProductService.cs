using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceApp.Application.ViewModels.EmployeePanel;

namespace EcommerceApp.Application.Interfaces
{
    public interface IProductService
    {
        Task AddProductAsync(ProductVM productVM);
        Task<ProductVM> GetProductAsync(int id);
        Task<ListProductForListVM> GetAllProductsAsync();
        Task<List<ProductVM>> GetAllProductsWithImagesAsync();
        Task<List<ProductVM>> GetProductsByCategoryNameAsync(string categoryName);
        Task UpdateProductAsync(ProductVM productVM);
        Task DeleteProductAsync(int id);
    }
}
