using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task AddProductAsync(Product product);
        Task<Product> GetProductAsync(int productId);
        IQueryable<Product> GetAllProducts();
        Task UpdateProductAsync(Product product);
        Task UpdateProductsAsync(List<Product> products);
        Task DeleteProductAsync(int productId);
    }
}
