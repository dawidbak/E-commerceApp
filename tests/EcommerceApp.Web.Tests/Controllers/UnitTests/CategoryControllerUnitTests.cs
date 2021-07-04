using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.Product;
using EcommerceApp.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace EcommerceApp.Web.Tests.Controllers.UnitTests
{
    public class CategoryControllerUnitTests
    {
        private readonly Mock<IProductService> _productService = new();
        private readonly CategoryController _sut;

        public CategoryControllerUnitTests()
        {
            _sut = new CategoryController(_productService.Object);
        }

        [Fact]
        public async Task Products_ReturnsViewResultWithAllProducts()
        {
            //Arrange
            var productDetailsForUserVMs = new List<ProductDetailsForUserVM>
            {
                new ProductDetailsForUserVM {Id = 1, Name ="test"},
                new ProductDetailsForUserVM {Id = 2, Name ="unit"}
            };
            var listProductDetailsForUserVM = new ListProductDetailsForUserVM { Products = productDetailsForUserVMs };

            _productService.Setup(x => x.GetProductsByCategoryNameAsync(It.IsAny<string>())).ReturnsAsync(listProductDetailsForUserVM);

            //Act
            var result = await _sut.Products("category");

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ListProductDetailsForUserVM>(viewResult.Model);
            Assert.Equal(listProductDetailsForUserVM.Products, model.Products);
        }

        [Fact]
        public async Task Products_ReturnsNotFoundWhenCategoryNameIsStringOrNull()
        {
            //Act
            var result = await _sut.Products(string.Empty);

            //Assert
            var redirectToNotFoundPage = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, redirectToNotFoundPage.StatusCode);
        }
    }
}
