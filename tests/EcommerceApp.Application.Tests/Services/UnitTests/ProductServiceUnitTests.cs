using System;
using Xunit;
using Moq;
using EcommerceApp.Application.Services;
using AutoMapper;
using EcommerceApp.Domain.Interfaces;
using System.Threading.Tasks;
using EcommerceApp.Domain.Models;
using EcommerceApp.Application.ViewModels.EmployeePanel;
using System.Collections.Generic;
using System.Linq;

namespace EcommerceApp.Application.Tests.Services.UnitTests
{
    public class ProductServiceUnitTests
    {
        private readonly ProductService _sut;
        private readonly Mock<IMapper> _mapper = new();
        private readonly Mock<IProductRepository> _productRepository = new();
        private readonly Mock<ICategoryRepository> _categoryRepository = new();

        public ProductServiceUnitTests()
        {
            _sut = new ProductService(_productRepository.Object,_categoryRepository.Object, _mapper.Object);
        }

        [Fact]
        public async Task AddProductAsync_ShouldAddProductMethodRunsOnce()
        {
            //Arrange
            var product = new Product() { Id = 1, Name = "Item", Description = "test", UnitPrice = 1.29M, UnitsInStock = 5 };
            var productVM = new ProductVM() { Id = 1, Name = "Item", Description = "test", UnitPrice = 1.29M, UnitsInStock = 5, CategoryName="mleko" };
            var category = new Category(){Id = 1, Name="mleko"};
            
            _categoryRepository.Setup(x => x.GetCategoryAsync(productVM.CategoryName)).ReturnsAsync(category);
            _mapper.Setup(x => x.Map<Product>(productVM)).Returns(product);

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
            var product = new Product() { Id = 1, Name = "Item", Description = "test", UnitPrice = 1.29M, UnitsInStock = 5 };
            var productVM = new ProductVM() { Id = 1, Name = "Item", Description = "test", UnitPrice = 1.29M, UnitsInStock = 5 };

            _productRepository.Setup(x => x.GetProductAsync(product.Id)).ReturnsAsync(product);
            _mapper.Setup(x => x.Map<ProductVM>(product)).Returns(productVM);

            //Act
            var result = await _sut.GetProductAsync(productVM.Id);

            //Assert
            Assert.Equal(productVM.Id, result.Id);
        }

        [Fact]
        public async Task UpdateProductAsync_ShouldUpdateMethodRunsOnce()
        {
            //Arrange
            var product = new Product() { Id = 1, Name = "Item", Description = "test", UnitPrice = 1.29M, UnitsInStock = 5 };
            var productVM = new ProductVM() { Id = 1, Name = "Item", Description = "test", UnitPrice = 1.29M, UnitsInStock = 5 };

            _mapper.Setup(x => x.Map<Product>(productVM)).Returns(product);

            //Act
            await _sut.UpdateProductAsync(productVM);

            //Assert
            _productRepository.Verify(x => x.UpdateProductAsync(It.IsAny<Product>()), Times.Once);
        }
    }
}
