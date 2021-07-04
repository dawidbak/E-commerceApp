using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.Home;
using EcommerceApp.Application.ViewModels.Product;
using EcommerceApp.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EcommerceApp.Web.Tests.Controllers.UnitTests
{
    public class HomeControllerUnitTests
    {
        private readonly Mock<ILogger<HomeController>> _logger = new();
        private readonly Mock<IHomeService> _homeService = new();
        private readonly HomeController _sut;

        public HomeControllerUnitTests()
        {
            _sut = new HomeController(_logger.Object, _homeService.Object);
        }

        [Fact]
        public async Task Index_ReturnsViewResultWithProducts()
        {
            //Arrange
            var homeVM = new HomeVM
            {
                Products = new ListProductDetailsForUserVM
                {
                    Products = new List<ProductDetailsForUserVM>
                    {
                        new ProductDetailsForUserVM { Id = 1, ImageUrl ="image"}
                    }
                }
            };

            _homeService.Setup(x => x.GetHomeVMForIndexAsync()).ReturnsAsync(homeVM);

            //Act
            var result = await _sut.Index();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<HomeVM>(viewResult.Model);
            Assert.Equal(homeVM.Products, model.Products);
        }
    }
}
