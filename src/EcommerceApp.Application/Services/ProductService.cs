using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.EmployeePanel;
using EcommerceApp.Application.ViewModels.Product;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IImageConverterService _imageConverterService;

        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository, IMapper mapper, IImageConverterService imageConverterService)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _imageConverterService = imageConverterService;
        }
        public async Task AddProductAsync(ProductVM productVM)
        {
            var product = _mapper.Map<Product>(productVM);
            var category = await _categoryRepository.GetCategoryAsync(productVM.CategoryName);
            product.CategoryId = category.Id;
            product.Image = await _imageConverterService.GetByteArrayFromImageAsync(productVM.ImageFormFile);
            await _productRepository.AddProductAsync(product);
        }

        public async Task DeleteProductAsync(int id)
        {
            await _productRepository.DeleteProductAsync(id);
        }

        public async Task<ListProductForListVM> GetAllProductsAsync()
        {
            var products = (await _productRepository.GetAllProductsAsync()).ToList();
            var productsVM = _mapper.Map<List<ProductForListVM>>(products);
            return new ListProductForListVM()
            {
                Products = productsVM
            };
        }

        public async Task<ListProductDetailsForUserVM> GetAllProductsWithImagesAsync()
        {
            var products = (await _productRepository.GetAllProductsAsync()).ToList();
            var productsVM = _mapper.Map<List<ProductDetailsForUserVM>>(products);
            for (int i = 0; i < productsVM.Count; i++)
            {
                productsVM[i].ImageUrl = _imageConverterService.GetImageUrlFromByteArray(products[i].Image);
            }
            return new ListProductDetailsForUserVM()
            {
                Products = productsVM
            };
        }

        public async Task<ProductVM> GetProductAsync(int id)
        {
            var product = await _productRepository.GetProductAsync(id);
            var productVM = _mapper.Map<ProductVM>(product);
            productVM.ImageUrl = _imageConverterService.GetImageUrlFromByteArray(product.Image);
            return productVM;
        }

        public async Task<ProductDetailsForUserVM> GetProductDetailsForUser(int id)
        {
            var product = await _productRepository.GetProductAsync(id);
            var productVM = _mapper.Map<ProductDetailsForUserVM>(product);
            productVM.ImageUrl = _imageConverterService.GetImageUrlFromByteArray(product.Image);
            return productVM;
        }

        public async Task<ListProductDetailsForUserVM> GetProductsByCategoryNameAsync(string categoryName)
        {
            var products = (await _productRepository.GetAllProductsAsync()).Where(p => p.CategoryName == categoryName).ToList();
            var productsVM = _mapper.Map<List<ProductDetailsForUserVM>>(products);
            for (int i = 0; i < productsVM.Count; i++)
            {
                productsVM[i].ImageUrl = _imageConverterService.GetImageUrlFromByteArray(products[i].Image);
            }
            return new ListProductDetailsForUserVM()
            {
                Products = productsVM
            };
        }

        public async Task UpdateProductAsync(ProductVM productVM)
        {
            var product = _mapper.Map<Product>(productVM);
            var category = await _categoryRepository.GetCategoryAsync(product.CategoryName);
            product.CategoryId = category.Id;
            product.Image = await _imageConverterService.GetByteArrayFromImageAsync(productVM.ImageFormFile);
            await _productRepository.UpdateProductAsync(product);
        }
    }
}
