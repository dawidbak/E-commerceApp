using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.Order;
using EcommerceApp.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace EcommerceApp.Web.Tests.Controllers.UnitTests
{
    public class OrderControllerUnitTests
    {
        private readonly Mock<IOrderService> _orderService = new();
        private readonly Mock<IConfiguration> _configuration = new();
        private readonly OrderController _sut;

        public OrderControllerUnitTests()
        {
            _sut = new OrderController(_orderService.Object, _configuration.Object);

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
        public async Task Checkout_Get_ReturnsNotFoundWhenIdIsNull()
        {
            //Act
            var result = await _sut.Checkout(customerId: null);

            //Assert
            var redirectToNotFoundPage = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, redirectToNotFoundPage.StatusCode);
        }

        [Fact]
        public async Task Checkout_Get_ReturnsViewResult()
        {
            //Assert
            var orderCheckoutVM = new OrderCheckoutVM { CartId = 1, CustomerId = 1, };

            _orderService.Setup(x => x.GetDataForOrderCheckoutAsync(orderCheckoutVM.CustomerId)).ReturnsAsync(orderCheckoutVM);

            //Act
            var result = await _sut.Checkout(orderCheckoutVM.CustomerId);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<OrderCheckoutVM>(viewResult.Model);
            Assert.Equal(orderCheckoutVM.CustomerId, model.CustomerId);
            Assert.Equal(orderCheckoutVM.CartId, model.CartId);
        }

        [Fact]
        public async Task Checkout_Post_ReturnsBadRequestWhenModelStateIsInvalid()
        {
            //Arrange
            var orderCheckoutVM = new OrderCheckoutVM { };
            _sut.ModelState.AddModelError("BadModel", "ChangeModel");

            //Act
            var result = await _sut.Checkout(orderCheckoutVM);

            //Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Checkout_Post_ReturnsRedirectToActionWhenModelStateIsValid()
        {
            //Arrange
            var orderCheckoutVM = new OrderCheckoutVM { CartId = 1, CustomerId = 1, };

            //Act
            var result = await _sut.Checkout(orderCheckoutVM);

            //Assert
            _orderService.Verify(x => x.AddOrderAsync(It.IsAny<OrderCheckoutVM>()), Times.Once);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("CheckoutSuccessful", redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task OrderHistory_ReturnsViewResultWithAllOrders()
        {
            //Arrange
            var customerOrderForListVMs = new List<CustomerOrderForListVM>
            {
                new CustomerOrderForListVM{ Id = 1, Price = 10},
                new CustomerOrderForListVM{ Id = 2, Price = 23}
            };
            var listCustomerOrderForListVM = new ListCustomerOrderForListVM
            {
                Orders = customerOrderForListVMs,
                TotalPages = 1,
                CurrentPage = 1,
            };

            _orderService.Setup(x => x.GetAllPaginatedCustomerOrdersAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(listCustomerOrderForListVM);

            //Act
            var result = await _sut.OrderHistory(1,"10");

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ListCustomerOrderForListVM>(viewResult.Model);
            Assert.Equal(listCustomerOrderForListVM.Orders, model.Orders);
        }

        [Fact]
        public async Task OrderDetails_ReturnsNotFoundWhenIdIsNull()
        {
            //Act
            var result = await _sut.OrderDetails(null);

            //Assert
            var redirectToNotFoundPage = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, redirectToNotFoundPage.StatusCode);
        }

        [Fact]
        public async Task OrderDetails_ReturnsViewResult()
        {
            //Assert
            var customerOrderDetailsVM = new CustomerOrderDetailsVM { Id = 1, Price = 23 };

            _orderService.Setup(x => x.GetCustomerOrderDetailsAsync(customerOrderDetailsVM.Id, It.IsAny<string>())).ReturnsAsync(customerOrderDetailsVM);

            //Act
            var result = await _sut.OrderDetails(customerOrderDetailsVM.Id);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CustomerOrderDetailsVM>(viewResult.Model);
            Assert.Equal(customerOrderDetailsVM.Id, model.Id);
            Assert.Equal(customerOrderDetailsVM.Price, model.Price);
        }
    }
}
