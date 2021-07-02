using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.Services;
using EcommerceApp.Application.ViewModels.Home;
using EcommerceApp.Application.ViewModels.Product;
using Moq;
using Xunit;

namespace EcommerceApp.Application.Tests.Services.UnitTests
{
    public class HomeServiceUnitTests
    {
        private readonly HomeService _sut;
        private readonly Mock<IProductService> _productService = new();

        public HomeServiceUnitTests()
        {
            _sut = new HomeService(_productService.Object);
        }

        [Fact]
        public async Task GetHomeVMForIndexAsync_ReturnsHomeVMAndCheckIfEqualToModel()
        {
            var products = new ListProductDetailsForUserVM
            {
                Products = new List<ProductDetailsForUserVM>()
                {
                    new ProductDetailsForUserVM{Id = 1, UnitPrice = 23, UnitsInStock= 2,Name ="unit"}
                }
            };
            var homeVM = new HomeVM { Products = products };

            _productService.Setup(x => x.GetRandomProductsWithImagesAsync(8)).ReturnsAsync(products);

            //Act
            var result = await _sut.GetHomeVMForIndexAsync();

            //Assert
            Assert.Equal(homeVM.Products, result.Products);
        }

    }
}
