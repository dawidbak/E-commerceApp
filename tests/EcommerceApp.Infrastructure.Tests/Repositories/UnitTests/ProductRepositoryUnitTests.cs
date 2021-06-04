using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceApp.Domain.Models;
using EcommerceApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EcommerceApp.Infrastructure.Tests.Repositories.UnitTests
{
    public class ProductRepositoryUnitTests
    {
        private readonly DbContextOptions<AppDbContext> _options;

        public ProductRepositoryUnitTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        }

        [Fact]
        public async Task AddProductAsync_CheckProductExistsAfterAdd()
        {
            //Arrange
            var product = new Product() { Id = 1, Name = "Item", Description = "test", UnitPrice = 1.29M, UnitsInStock = 5 };

            using (var context = new AppDbContext(_options))
            {
                //Act
                await context.Database.EnsureCreatedAsync();
                var sut = new ProductRepository(context);
                await sut.AddProductAsync(product);
                var result = await context.Products.FindAsync(product.Id);

                //Assert
                Assert.Equal(product, result);
            }
        }

        [Fact]
        public async Task GetProductAsync_FetchProductAndVerifyIfEqualToModel()
        {
            //Arrange
            var product = new Product() { Id = 1, Name = "Item", Description = "test", UnitPrice = 1.29M, UnitsInStock = 5 };

            using (var context = new AppDbContext(_options))
            {
                //Act
                await context.Database.EnsureCreatedAsync();
                await context.Products.AddAsync(product);
                await context.SaveChangesAsync();
                var sut = new ProductRepository(context);
                var result = await sut.GetProductAsync(product.Id);

                //Assert
                Assert.Equal(product, result);
            }
        }

        [Fact]
        public async Task GetAllProductsAsync_FetchListOfProductsAndVerifyIfAreEqualToModels()
        {
            //Arrange
            var product1 = new Product() { Id = 1, Name = "Item", Description = "test", UnitPrice = 1.29M, UnitsInStock = 5 };
            var product2 = new Product() { Id = 2, Name = "ItemX", Description = "testX", UnitPrice = 5.49M, UnitsInStock = 20 };
            List<Product> products = new() { product1, product2 };

            using (var context = new AppDbContext(_options))
            {
                //Act
                await context.Database.EnsureCreatedAsync();
                await context.AddRangeAsync(products);
                await context.SaveChangesAsync();
                var sut = new ProductRepository(context);
                var results = await sut.GetAllProductsAsync();

                //Assert
                Assert.Equal(products, results);
            }
        }

        [Fact]
        public async Task UpdateProductAsync_ShouldUpdateProductProperties()
        {
            //Arrange
            var product1 = new Product() { Id = 1, Name = "Item", Description = "test", UnitPrice = 1.29M, UnitsInStock = 5 };
            var product2 = new Product() { Id = 1, Name = "ItemX", Description = "testX", UnitPrice = 5.49M, UnitsInStock = 20 };

            using (var context = new AppDbContext(_options))
            {
                //Act
                await context.Database.EnsureCreatedAsync();
                await context.AddAsync(product1);
                await context.SaveChangesAsync();
            }
            using (var context = new AppDbContext(_options))
            {
                //Act
                await context.Database.EnsureCreatedAsync();
                var sut = new ProductRepository(context);
                await sut.UpdateProductAsync(product2);
                var result = await context.Products.FindAsync(product1.Id);

                //Assert
                Assert.Equal(product2.Id, result.Id);
                Assert.Equal(product2.Name, result.Name);
                Assert.Equal(product2.Description, result.Description);
                Assert.Equal(product2.UnitPrice, result.UnitPrice);
                Assert.Equal(product2.UnitsInStock, result.UnitsInStock);
            }
        }

        [Fact]
        public async Task DeleteProductAsync_ShouldProductBeNullAfterDelete()
        {
            //Arrange
            var product = new Product() { Id = 1, Name = "Item", Description = "test", UnitPrice = 1.29M, UnitsInStock = 5 };

            using (var context = new AppDbContext(_options))
            {
                //Act
                await context.Database.EnsureCreatedAsync();
                await context.Products.AddAsync(product);
                await context.SaveChangesAsync();
                var sut = new ProductRepository(context);
                await sut.DeleteProductAsync(product.Id);
                var result = await context.Products.FindAsync(product.Id);

                //Assert
                Assert.Null(result);
            }
        }
    }
}
