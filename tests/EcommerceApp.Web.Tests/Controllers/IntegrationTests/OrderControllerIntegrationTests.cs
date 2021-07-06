using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace EcommerceApp.Web.Tests.Controllers.IntegrationTests
{
    public class OrderControllerIntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _sut;
        private readonly HttpClient _clientAuth;
        private readonly HttpClient _clientUnauth;

        public OrderControllerIntegrationTests(WebApplicationFactory<Startup> sut)
        {
            _sut = sut;
            _clientAuth = sut.GetCustomerHttpClient();
            _clientUnauth = sut.GetGuestHttpClient();
        }

        [Theory]
        [InlineData("Order/Checkout")]
        [InlineData("Order/OrderHistory")]
        [InlineData("Order/OrderDetails")]
        public async Task Get_EndpointsReturnRedirectForUnauthenticatedUser(string url)
        {
            // Act
            var response = await _clientUnauth.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.StartsWith("http://localhost/Identity/Account/Login", response.Headers.Location.OriginalString);
        }

        [Theory]
        [InlineData("Order/Checkout?customerId=1")]
        [InlineData("Order/OrderHistory")]
        [InlineData("Order/OrderHistory?pageNumber=1&pageSize=5")]
        [InlineData("Order/OrderDetails/1")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentTypeForAuthenticatedUser(string url)
        {
            // Act
            var response = await _clientAuth.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task OrderDetails_EndpointReturnNotFoundWhenIdHasNoValueForAuthenticatedUser()
        {
            // Act
            var response = await _clientAuth.GetAsync("Order/OrderDetails");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

                [Fact]
        public async Task Checkout_Get_EndpointReturnBadRequestWhenCustomerIdHasNoValueForAuthenticatedUser()
        {
            // Act
            var response = await _clientAuth.GetAsync("Order/Checkout");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Checkout_Post_EndpointReturnRedirectForUnauthenticatedUser()
        {
            // Arrange
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "Order/Checkout?customerId=1");

            // Act
            var response = await _clientUnauth.SendAsync(postRequest);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.StartsWith("http://localhost/Identity/Account/Login", response.Headers.Location.OriginalString);
        }

        [Fact]
        public async Task Checkout_Post_EndpointReturnRedirectResultForAuthenticatedUser()
        {
            // Arrange
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "Order/Checkout?customerId=1");
            var orderCheckoutVM = new Dictionary<string, string>
            {
                { "CartId", "1" },
                { "CustomerId", "1" },
                { "TotalPrice", "10" },
                { "FirstName", "Test" },
                { "LastName", "Test" },
                { "City", "Test" },
                { "PostalCode", "11111" },
                { "Address", "st. Test 2" },
                { "Email", "test@example.com" },
                { "PhoneNumber", "123456789" },
                { "CartItems[0].ProductId", "1" },
                { "CartItems[0].Quantity", "1" },
            };
            postRequest.Content = new FormUrlEncodedContent(orderCheckoutVM);

            // Act
            var response = await _clientAuth.SendAsync(postRequest);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        }

        [Fact]
        public async Task Checkout_Post_EndpointReturnBadRequestWhenModelIsNotValidForAuthenticatedUser()
        {
            // Arrange
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "Order/Checkout?customerId=1");
            var orderCheckoutVM = new Dictionary<string, string>();
            postRequest.Content = new FormUrlEncodedContent(orderCheckoutVM);

            // Act
            var response = await _clientAuth.SendAsync(postRequest);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

    }
}
