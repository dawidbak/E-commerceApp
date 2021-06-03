using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _appDbContext;
        public ProductRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task AddProductAsync(Product product)
        {
            await _appDbContext.AddAsync(product);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int productId)
        {
            var product = await _appDbContext.Products.FindAsync(productId);
            if (product != null)
            {
                _appDbContext.Products.Remove(product);
                await _appDbContext.SaveChangesAsync();
            }
        }

        public async Task<IQueryable<Product>> GetAllProductsAsync()
        {
            return (await _appDbContext.Products.ToListAsync()).AsQueryable();
        }

        public async Task<Product> GetProductAsync(int productId)
        {
            return await _appDbContext.Products.FindAsync(productId);
        }

        public async Task UpdateProductAsync(Product product)
        {
            _appDbContext.Products.Update(product);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
