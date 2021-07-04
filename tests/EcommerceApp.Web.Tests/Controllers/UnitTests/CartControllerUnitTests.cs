using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.Cart;
using EcommerceApp.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace EcommerceApp.Web.Tests.Controllers.UnitTests
{
    public class CartControllerUnitTests
    {
        private readonly Mock<ICartService> _cartService = new();
        private readonly CartController _sut;

        public CartControllerUnitTests()
        {
            _sut = new CartController(_cartService.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1")
            }));

            _sut.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }

        [Fact]
        public async Task Index_ReturnsCorrectViewResultWithAllCartItems()
        {
            //Arrange
            var cartItemForListVMs = new List<CartItemForListVM>
            {
                new CartItemForListVM{Id = 1, Name = "test", ProductId = 1},
                new CartItemForListVM{Id = 2, Name = "unit", ProductId = 2}
            };
            var listCartItemForListVM = new ListCartItemForListVM
            {
                CartItems = cartItemForListVMs
            };

            _cartService.Setup(x => x.GetAllCartItemsForCurrentUserAsync(It.IsAny<string>())).ReturnsAsync(listCartItemForListVM);

            //Act
            var results = await _sut.Index();
            //Assert
            Assert.NotNull(results);
            var viewResult = Assert.IsType<ViewResult>(results);
            var model = Assert.IsAssignableFrom<ListCartItemForListVM>(viewResult.Model);
            Assert.Equal(model.CartItems, listCartItemForListVM.CartItems);
        }

        [Fact]
        public async Task AddToCart_RedirectsToActionResult()
        {
            //Act
            var result = await _sut.AddToCart(id: 1, quantity: 2);

            //Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            _cartService.Verify(x => x.AddCartItemAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task IncreaseQuantityCartItemByOne_ReturnsNotFoundWhenIdIsNull()
        {
            //Act
            var result = await _sut.IncreaseQuantityCartItemByOne(null);

            //Assert
            var redirectToNotFoundPage = Assert.IsType<NotFoundResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, redirectToNotFoundPage.StatusCode);
        }

        [Fact]
        public async Task IncreaseQuantityCartItemByOne_RedirectsToActionResult()
        {
            //Act
            var result = await _sut.IncreaseQuantityCartItemByOne(1);

            //Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            _cartService.Verify(x => x.IncreaseQuantityCartItemByOneAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task DecreaseQuantityCartItemByOne_ReturnsNotFoundWhenIdIsNull()
        {
            //Act
            var result = await _sut.DecreaseQuantityCartItemByOne(null);

            //Assert
            var redirectToNotFoundPage = Assert.IsType<NotFoundResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, redirectToNotFoundPage.StatusCode);
        }

        [Fact]
        public async Task DecreaseQuantityCartItemByOne_RedirectsToActionResult()
        {
            //Act
            var result = await _sut.DecreaseQuantityCartItemByOne(1);

            //Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            _cartService.Verify(x => x.DecreaseQuantityCartItemByOneAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task DeleteCartItem_ReturnsNotFoundWhenIdIsNull()
        {
            //Act
            var result = await _sut.DeleteCartItem(null);

            //Assert
            var redirectToNotFoundPage = Assert.IsType<NotFoundResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, redirectToNotFoundPage.StatusCode);
        }

        [Fact]
        public async Task DeleteCartItem_RedirectsToActionResult()
        {
            //Act
            var result = await _sut.DeleteCartItem(1);

            //Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            _cartService.Verify(x => x.DeleteCartItemAsync(It.IsAny<int>()), Times.Once);
        }
    }
}
