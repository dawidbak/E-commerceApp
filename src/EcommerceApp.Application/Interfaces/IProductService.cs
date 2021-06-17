using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceApp.Application.ViewModels.EmployeePanel;
using EcommerceApp.Application.ViewModels.Product;

namespace EcommerceApp.Application.Interfaces
{
    public interface IProductService
    {
        Task AddProductAsync(ProductVM productVM);
        Task<ProductVM> GetProductAsync(int id);
        Task<ProductDetailsForUserVM> GetProductDetailsForUser(int id);
        Task<ListProductForListVM> GetAllProductsAsync();
        Task<ListProductDetailsForUserVM> GetAllProductsWithImagesAsync();
        Task<ListProductDetailsForUserVM> GetProductsByCategoryNameAsync(string categoryName);
        Task<ListProductForListVM> GetAllPaginatedProductsAsync(int pageSize, int pageNumber);
        Task UpdateProductAsync(ProductVM productVM);
        Task DeleteProductAsync(int id);
    }
}
