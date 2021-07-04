using System;
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
    public class ProductControllerUnitTests
    {
        private readonly Mock<IProductService> _productService = new();
        private readonly ProductController _sut;

        public ProductControllerUnitTests()
        {
            _sut = new ProductController(_productService.Object);
        }

        [Fact]
        public async Task Product_ReturnsNotFoundWhenIdIsNull()
        {
            //Act
            var result = await _sut.Product(null);

            //Assert
            var redirectToNotFoundPage = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, redirectToNotFoundPage.StatusCode);
        }

        [Fact]
        public async Task Product_ReturnsViewResult()
        {
            //Assert
            var productDetailsForUserVM = new ProductDetailsForUserVM { Id = 1, UnitPrice = 23 };

            _productService.Setup(x => x.GetProductDetailsForUserAsync(productDetailsForUserVM.Id)).ReturnsAsync(productDetailsForUserVM);

            //Act
            var result = await _sut.Product(productDetailsForUserVM.Id);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ProductDetailsForUserVM>(viewResult.Model);
            Assert.Equal(productDetailsForUserVM.Id, model.Id);
            Assert.Equal(productDetailsForUserVM.UnitPrice, model.UnitPrice);
        }
    }
}
