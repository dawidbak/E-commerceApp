using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.Services;
using EcommerceApp.Application.ViewModels;
using EcommerceApp.Application.ViewModels.EmployeePanel;
using EcommerceApp.Application.ViewModels.Product;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Microsoft.AspNetCore.Http;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace EcommerceApp.Application.Tests.Services.UnitTests
{
    public class ProductServiceUnitTests
    {
        private readonly ProductService _sut;
        private readonly Mock<IMapper> _mapper = new();
        private readonly Mock<IProductRepository> _productRepository = new();
        private readonly Mock<ICategoryRepository> _categoryRepository = new();
        private readonly Mock<IImageConverterService> _imageConverterService = new();
        private readonly Mock<IFormFile> _fileMock = new();
        private readonly Mock<IPaginationService<ProductForListVM>> _paginationService = new();

        public ProductServiceUnitTests()
        {
            _sut = new ProductService(_productRepository.Object, _categoryRepository.Object, _mapper.Object, _imageConverterService.Object, _paginationService.Object);
        }

        [Fact]
        public async Task AddProductAsync_ShouldAddProductMethodRunsOnce()
        {
            //Arrange
            var product = new Product() { Id = 1, Name = "Item", Description = "test", UnitPrice = 1.29M, UnitsInStock = 5, Image = new byte[] { 2, 3, 4 } };
            var productVM = new ProductVM() { Id = 1, Name = "Item", Description = "test", UnitPrice = 1.29M, UnitsInStock = 5, CategoryName = "mleko" };

            _mapper.Setup(x => x.Map<Product>(productVM)).Returns(product);
            _imageConverterService.Setup(x => x.GetByteArrayFromImageAsync(_fileMock.Object)).ReturnsAsync(product.Image);

            //Act
            await _sut.AddProductAsync(productVM);

            //Assert
            _productRepository.Verify(x => x.AddProductAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task DeleteProductAsync_ShouldDeleteMethodRunsOnce()
        {
            //Arrange
            var product = new ProductVM() { Id = 1 };

            //Act
            await _sut.DeleteProductAsync(product.Id);

            //Assert
            _productRepository.Verify(x => x.DeleteProductAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetAllPaginatedProductsAsync_ReturnsListProductForListVMAndCheckIfEqualToModel()
        {
            //Arrange
            List<Product> products = new()
            {
                new Product() { Id = 1, Name = "Item", Description = "test", UnitPrice = 1.29M, UnitsInStock = 5 },
                new Product() { Id = 2, Name = "ItemX", Description = "testX", UnitPrice = 2.39M, UnitsInStock = 20 },
            };
            List<ProductForListVM> productForListVMs = new()
            {
                new ProductForListVM() { Id = 1, Name = "Item", UnitPrice = 1.29M, UnitsInStock = 5 },
                new ProductForListVM() { Id = 2, Name = "ItemX", UnitPrice = 2.39M, UnitsInStock = 20 },
            };

            PaginatedVM<ProductForListVM> paginatedVM = new()
            {
                Items = productForListVMs,
                CurrentPage = 1,
                TotalPages = 2,
            };
            ListProductForListVM listProductForListVM = new()
            {
                Products = productForListVMs,
                CurrentPage = 1,
                TotalPages = 2,
            };
            _mapper.Setup(x => x.ConfigurationProvider).Returns(() => new MapperConfiguration(cfg =>
            { cfg.CreateMap<Product, ProductForListVM>(); }));
            _productRepository.Setup(x => x.GetAllProducts()).Returns(products.AsQueryable);
            _paginationService.Setup(x => x.CreateAsync(It.IsAny<IQueryable<ProductForListVM>>(), 1, 10)).ReturnsAsync(paginatedVM);
            _mapper.Setup(x => x.Map<ListProductForListVM>(paginatedVM)).Returns(listProductForListVM);

            //Act
            var result = await _sut.GetAllPaginatedProductsAsync(10, 1);

            //Assert
            Assert.Equal(listProductForListVM, result);
        }

        [Fact]
        public async Task GetRandomProductsWithImagesAsync_ReturnsListProductDetailsForUserVMAndCheckIfEqualToModel()
        {
            //Arrange
            var products = new List<Product>()
            {
                new Product {Id = 1, Image = new byte[]{1,2}}
            };
            var productsQuery = products.AsQueryable().BuildMock();

            var productDetailsForUserVMs = new List<ProductDetailsForUserVM>()
            {
                new ProductDetailsForUserVM {Id = 1, ImageUrl = "ads"}
            };
            var listProductDetailsForUserVM = new ListProductDetailsForUserVM { Products = productDetailsForUserVMs };

            _productRepository.Setup(x => x.GetAllProducts()).Returns(productsQuery.Object);
            _mapper.Setup(x => x.Map<List<ProductDetailsForUserVM>>(products)).Returns(productDetailsForUserVMs);
            _imageConverterService.Setup(x => x.GetImageUrlFromByteArray(products[0].Image)).Returns(productDetailsForUserVMs[0].ImageUrl);

            //Act
            var result = await _sut.GetRandomProductsWithImagesAsync(1);

            //Assert
            Assert.Equal(listProductDetailsForUserVM.Products, result.Products);
        }

        [Fact]
        public async Task GetProductAsync_FetchProductAndVerifyIfEqualToModel()
        {
            //Arrange
            byte[] numbers = { 0, 16, 2, 3, 4 };
            var product = new Product() { Id = 1, Name = "Item", Description = "test", UnitPrice = 1.29M, UnitsInStock = 5, Image = numbers };
            var productVM = new ProductVM() { Id = 1, Name = "Item", Description = "test", UnitPrice = 1.29M, UnitsInStock = 5, ImageUrl = "2sdasds" };

            _productRepository.Setup(x => x.GetProductAsync(product.Id)).ReturnsAsync(product);
            _mapper.Setup(x => x.Map<ProductVM>(product)).Returns(productVM);
            _imageConverterService.Setup(x => x.GetImageUrlFromByteArray(product.Image)).Returns(productVM.ImageUrl);

            //Act
            var result = await _sut.GetProductAsync(productVM.Id);

            //Assert
            Assert.Equal(productVM, result);
        }

        [Fact]
        public async Task GetProductDetailsForUser_ReturnsProductDetailsForUserVMAndCheckIfEqualToModel()
        {
            //Arrange
            var product = new Product { Id = 10, Name = "asdasdas", UnitsInStock = 2, UnitPrice = 3.23m, Image = new byte[] { 1, 2, 3 } };
            var productDetailsVM = new ProductDetailsForUserVM { Id = 10, Name = "asdasdas", UnitsInStock = 2, UnitPrice = 3.23m, ImageUrl = "adas" };

            _productRepository.Setup(x => x.GetProductAsync(product.Id)).ReturnsAsync(product);
            _mapper.Setup(x => x.Map<ProductDetailsForUserVM>(product)).Returns(productDetailsVM);
            _imageConverterService.Setup(x => x.GetImageUrlFromByteArray(product.Image)).Returns(productDetailsVM.ImageUrl);

            //Act
            var result = await _sut.GetProductDetailsForUserAsync(product.Id);

            //Assert
            Assert.Equal(productDetailsVM, result);
        }

        [Fact]
        public async Task GetProductsByCategoryNameAsync_ReturnsListProductDetailsForUserVMAndCheckIfEqualToModel()
        {
            //Arrange
            var category = new Category { Name = "test" };
            var products = new List<Product>()
            {
                new Product { Id = 10, Name = "asdasdas", UnitsInStock = 2, UnitPrice = 3.23m, Image = new byte[] { 1, 4, 3 }, Category = category },
                new Product { Id = 11, Name = "asdasxz", UnitsInStock = 3, UnitPrice = 3.52m, Image = new byte[] { 1, 2, 3 }, Category = category }
            };
            var productDetailsForUserVMs = new List<ProductDetailsForUserVM>()
            {
                new ProductDetailsForUserVM { Id = 10, Name = "asdasdas", UnitsInStock = 2, UnitPrice = 3.23m, ImageUrl = "123" },
                new ProductDetailsForUserVM { Id = 11, Name = "asdasxz", UnitsInStock = 3, UnitPrice = 3.52m, ImageUrl = "124" }
            };
            var productsQuery = products.AsQueryable().BuildMock();

            var listProductDetailsForUserVM = new ListProductDetailsForUserVM
            {
                Products = productDetailsForUserVMs
            };

            _productRepository.Setup(x => x.GetAllProducts()).Returns(productsQuery.Object);
            _mapper.Setup(x => x.Map<List<ProductDetailsForUserVM>>(products)).Returns(productDetailsForUserVMs);

            //Act
            var result = await _sut.GetProductsByCategoryNameAsync(category.Name);

            //Assert
            Assert.Equal(listProductDetailsForUserVM.Products, result.Products);
            _imageConverterService.Verify(x => x.GetImageUrlFromByteArray(It.IsAny<byte[]>()), Times.Exactly(2));
        }

        [Fact]
        public async Task UpdateProductAsync_ShouldUpdateMethodRunsOnce()
        {
            //Arrange
            var product = new Product() { Id = 1, Name = "Item", Description = "test", UnitPrice = 1.29M, UnitsInStock = 5 };
            var productVM = new ProductVM() { Id = 1, Name = "Item", Description = "test", UnitPrice = 1.29M, UnitsInStock = 5 };
            var category = new Category() { Id = 1, Name = "mleko" };

            _mapper.Setup(x => x.Map<Product>(productVM)).Returns(product);
            _imageConverterService.Setup(x => x.GetByteArrayFromImageAsync(_fileMock.Object)).ReturnsAsync(product.Image);

            //Act
            await _sut.UpdateProductAsync(productVM);

            //Assert
            _productRepository.Verify(x => x.UpdateProductAsync(It.IsAny<Product>()), Times.Once);
        }
    }
}
