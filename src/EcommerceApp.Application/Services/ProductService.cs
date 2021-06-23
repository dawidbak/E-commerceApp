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
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IImageConverterService _imageConverterService;
        private readonly IPaginationService<ProductForListVM> _paginationService;

        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository, IMapper mapper, IImageConverterService imageConverterService,
         IPaginationService<ProductForListVM> paginationService)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _imageConverterService = imageConverterService;
            _paginationService = paginationService;
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

        public async Task<ListProductDetailsForUserVM> GetRandomProductsWithImagesAsync(int number)
        {
            var products = _productRepository.GetAllProducts();
            int count = await products.CountAsync();
            if (count > number)
            {
                var random = new Random();
                products = products.OrderBy(x => x.Id).Skip(random.Next(count + 1 - number)).Take(number);
            }
            var randomProducts = await products.ToListAsync();
            var productsVM = _mapper.Map<List<ProductDetailsForUserVM>>(randomProducts);
            for (int i = 0; i < productsVM.Count; i++)
            {
                productsVM[i].ImageUrl = _imageConverterService.GetImageUrlFromByteArray(randomProducts[i].Image);
            }
            return new ListProductDetailsForUserVM()
            {
                Products = productsVM
            };
        }

        public async Task<ListProductForListVM> GetAllPaginatedProductsAsync(int pageSize, int pageNumber)
        {
            var productsVM = _productRepository.GetAllProducts().ProjectTo<ProductForListVM>(_mapper.ConfigurationProvider);
            var paginatedVM = await _paginationService.CreateAsync(productsVM, pageNumber, pageSize);
            return _mapper.Map<ListProductForListVM>(paginatedVM);
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
            var products = await _productRepository.GetAllProducts().Where(p => p.CategoryName == categoryName).ToListAsync();
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
