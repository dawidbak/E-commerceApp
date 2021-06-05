using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.Services;
using EcommerceApp.Application.ViewModels.EmployeePanel;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Microsoft.AspNetCore.Http;
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

        public ProductServiceUnitTests()
        {
            _sut = new ProductService(_productRepository.Object, _categoryRepository.Object, _mapper.Object, _imageConverterService.Object);
        }

        [Fact]
        public async Task AddProductAsync_ShouldAddProductMethodRunsOnce()
        {
            //Arrange
            var product = new Product() { Id = 1, Name = "Item", Description = "test", UnitPrice = 1.29M, UnitsInStock = 5, Image = new byte[]{2,3,4} };
            var productVM = new ProductVM() { Id = 1, Name = "Item", Description = "test", UnitPrice = 1.29M, UnitsInStock = 5, CategoryName = "mleko" };
            var category = new Category() { Id = 1, Name = "mleko" };

            _categoryRepository.Setup(x => x.GetCategoryAsync(productVM.CategoryName)).ReturnsAsync(category);
            _mapper.Setup(x => x.Map<Product>(productVM)).Returns(product);
            _imageConverterService.Setup(x => x.GetByteArrayFromImageAsync(_fileMock.Object)).ReturnsAsync(product.Image);

            //Act
            await _sut.AddProductAsync(productVM);

            //Assert
            _productRepository.Verify(x => x.AddProductAsync(It.IsAny<Product>()), Times.Once);
            _imageConverterService.Verify(x => x.GetByteArrayFromImageAsync(It.IsAny<IFormFile>()), Times.Once);
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
        public async Task GetAllProductsAsync_FetchListOfProductsAndVerifyIfAreEqualToModels()
        {
            //Arrange
            List<Product> products = new()
            {
                new Product() { Id = 1, Name = "Item", Description = "test", UnitPrice = 1.29M, UnitsInStock = 5 },
                new Product() { Id = 2, Name = "ItemX", Description = "testX", UnitPrice = 2.39M, UnitsInStock = 20 },
            };
            List<ProductVM> productsVM = new()
            {
                new ProductVM() { Id = 1, Name = "Item", Description = "test", UnitPrice = 1.29M, UnitsInStock = 5 },
                new ProductVM() { Id = 2, Name = "ItemX", Description = "testX", UnitPrice = 2.39M, UnitsInStock = 20 },
            };

            _productRepository.Setup(x => x.GetAllProductsAsync()).ReturnsAsync(products.AsQueryable);
            _mapper.Setup(x => x.Map<List<ProductVM>>(products)).Returns(productsVM);

            //Act
            var results = await _sut.GetAllProductsAsync();

            //Assert
            for (int i = 0; i < results.Count; i++)
            {
                Assert.Equal(productsVM[i].Id, results[i].Id);
                Assert.Equal(productsVM[i].Name, results[i].Name);
                Assert.Equal(productsVM[i].Description, results[i].Description);
                Assert.Equal(productsVM[i].UnitPrice, results[i].UnitPrice);
                Assert.Equal(productsVM[i].UnitsInStock, results[i].UnitsInStock);
            }
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
            Assert.Equal(productVM.Id, result.Id);
            Assert.Equal(productVM.Name, result.Name);
            Assert.Equal(productVM.Description, result.Description);
            Assert.Equal(productVM.UnitPrice, result.UnitPrice);
            Assert.Equal(productVM.UnitsInStock, result.UnitsInStock);
            Assert.Equal(productVM.ImageUrl, result.ImageUrl);
            _imageConverterService.Verify(x => x.GetImageUrlFromByteArray(It.IsAny<byte[]>()), Times.Once);
        }

        [Fact]
        public async Task UpdateProductAsync_ShouldUpdateMethodRunsOnce()
        {
            //Arrange
            var product = new Product() { Id = 1, Name = "Item", Description = "test", UnitPrice = 1.29M, UnitsInStock = 5 };
            var productVM = new ProductVM() { Id = 1, Name = "Item", Description = "test", UnitPrice = 1.29M, UnitsInStock = 5 };
            var category = new Category() { Id = 1, Name = "mleko" };

            _mapper.Setup(x => x.Map<Product>(productVM)).Returns(product);
            _categoryRepository.Setup(x => x.GetCategoryAsync(productVM.CategoryName)).ReturnsAsync(category);
            _imageConverterService.Setup(x => x.GetByteArrayFromImageAsync(_fileMock.Object)).ReturnsAsync(product.Image);

            //Act
            await _sut.UpdateProductAsync(productVM);

            //Assert
            _productRepository.Verify(x => x.UpdateProductAsync(It.IsAny<Product>()), Times.Once);
            _imageConverterService.Verify(x => x.GetByteArrayFromImageAsync(It.IsAny<IFormFile>()), Times.Once);
        }
    }
}
