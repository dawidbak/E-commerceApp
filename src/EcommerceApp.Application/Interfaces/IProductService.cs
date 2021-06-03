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
        Task<List<ProductVM>> GetAllProductsAsync();
        Task UpdateProductAsync(ProductVM productVM);
        Task DeleteProductAsync(int id);
    }
}
