using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.EmployeePanel;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }
        public async Task AddProductAsync(ProductVM productVM)
        {
            var product = _mapper.Map<Product>(productVM);
            await _productRepository.AddProductAsync(product);
            
        }

        public async Task DeleteProductAsync(int productVMId)
        {
            await _productRepository.DeleteProductAsync(productVMId);
        }

        public async Task<List<ProductVM>> GetAllProductsAsync()
        {
            var products =  (await _productRepository.GetAllProductsAsync()).ToList();
            return _mapper.Map<List<ProductVM>>(products);
        }

        public async Task<ProductVM> GetProductAsync(int productVMId)
        {
            var product = await _productRepository.GetProductAsync(productVMId);
            return _mapper.Map<ProductVM>(product);
        }

        public async Task UpdateProductAsync(ProductVM productVM)
        {
            var product = _mapper.Map<Product>(productVM);
            await _productRepository.UpdateProductAsync(product);
        }
    }
}
