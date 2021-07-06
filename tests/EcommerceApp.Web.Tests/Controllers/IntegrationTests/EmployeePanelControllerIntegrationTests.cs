using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace EcommerceApp.Web.Tests.Controllers.IntegrationTests
{
    public class EmployeePanelControllerIntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _sut;
        private readonly HttpClient _clientAuth;
        private readonly HttpClient _clientUnauth;

        public EmployeePanelControllerIntegrationTests(WebApplicationFactory<Startup> sut)
        {
            _sut = sut;
            _clientAuth = _sut.GetEmployeeHttpClient();
            _clientUnauth = _sut.GetGuestHttpClient();
        }

        [Theory]
        [InlineData("EmployeePanel/Index")]
        [InlineData("EmployeePanel/Categories")]
        [InlineData("EmployeePanel/Products")]
        [InlineData("EmployeePanel/Orders")]
        [InlineData("EmployeePanel/OrderDetails/1")]
        [InlineData("EmployeePanel/AddCategory")]
        [InlineData("EmployeePanel/AddProduct")]
        [InlineData("EmployeePanel/EditCategory/1")]
        [InlineData("EmployeePanel/EditProduct/1")]
        [InlineData("EmployeePanel/DeleteCategory/1")]
        [InlineData("EmployeePanel/DeleteProduct/1")]
        [InlineData("EmployeePanel/DeleteOrder/1")]
        public async Task Get_EndpointsReturnRedirectForUnauthenticatedUser(string url)
        {
            // Act
            var response = await _clientUnauth.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.StartsWith("http://localhost/Identity/Account/Login", response.Headers.Location.OriginalString);
        }

        [Theory]
        [InlineData("EmployeePanel/Index")]
        [InlineData("EmployeePanel/Categories")]
        [InlineData("EmployeePanel/Products")]
        [InlineData("EmployeePanel/Orders")]
        [InlineData("EmployeePanel/Categories?PageSize=10&SelectedValue=Id&SearchString=1")]
        [InlineData("EmployeePanel/Products?PageSize=10&SelectedValue=Id&SearchString=1")]
        [InlineData("EmployeePanel/Orders?PageSize=10&SelectedValue=Id&SearchString=1")]
        [InlineData("EmployeePanel/OrderDetails/1")]
        [InlineData("EmployeePanel/AddCategory")]
        [InlineData("EmployeePanel/AddProduct")]
        [InlineData("EmployeePanel/EditCategory/1")]
        [InlineData("EmployeePanel/EditProduct/1")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentTypeForAuthenticatedUser(string url)
        {
            // Act
            var response = await _clientAuth.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData("EmployeePanel/DeleteCategory/1")]
        [InlineData("EmployeePanel/DeleteProduct/1")]
        [InlineData("EmployeePanel/DeleteOrder/1")]
        public async Task Get_EndpointsReturnRedirectForAuthenticatedUser(string url)
        {
            // Act
            var response = await _clientAuth.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        }

        [Theory]
        [InlineData("EmployeePanel/OrderDetails")]
        [InlineData("EmployeePanel/UpdateCategory")]
        [InlineData("EmployeePanel/UpdateProduct")]
        [InlineData("EmployeePanel/DeleteCategory")]
        [InlineData("EmployeePanel/DeleteProduct")]
        [InlineData("EmployeePanel/DeleteOrder")]
        public async Task Get_EndpointsReturnNotFoundWhenIdHasNoValueForAuthenticatedUser(string url)
        {
            // Act
            var response = await _clientAuth.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData("EmployeePanel/AddCategory")]
        [InlineData("EmployeePanel/AddProduct")]
        [InlineData("EmployeePanel/EditCategory/1")]
        [InlineData("EmployeePanel/EditProduct/1")]
        public async Task Post_EndpointsReturnRedirectForUnauthenticatedUser(string url)
        {
            // Arrange
            var postRequest = new HttpRequestMessage(HttpMethod.Post, url);

            // Act
            var response = await _clientUnauth.SendAsync(postRequest);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.StartsWith("http://localhost/Identity/Account/Login", response.Headers.Location.OriginalString);
        }

        [Theory]
        [InlineData("EmployeePanel/AddProduct")]
        [InlineData("EmployeePanel/EditProduct/1")]
        public async Task ProductActions_Post_EndpointsReturnRedirectResultForAuthenticatedUser(string url)
        {
            // Arrange
            var postRequest = new HttpRequestMessage(HttpMethod.Post, url);
            var productVM = new Dictionary<string, string>
            {
                { "Name", "product" },
                { "Description", "good" },
                { "UnitPrice", "2" },
                { "UnitsInStock", "2" },
            };
            postRequest.Content = new FormUrlEncodedContent(productVM);

            // Act
            var response = await _clientAuth.SendAsync(postRequest);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        }

        [Theory]
        [InlineData("EmployeePanel/AddCategory")]
        [InlineData("EmployeePanel/EditCategory/1")]
        public async Task CategoryActions_Post_EndpointsReturnRedirectResultForAuthenticatedUser(string url)
        {
            // Arrange
            var postRequest = new HttpRequestMessage(HttpMethod.Post, url);
            var categoryVM = new Dictionary<string, string>
            {
                { "Name", "test" },
                { "Description", "goodcategory" }
            };
            postRequest.Content = new FormUrlEncodedContent(categoryVM);

            // Act
            var response = await _clientAuth.SendAsync(postRequest);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        }

        [Theory]
        [InlineData("EmployeePanel/AddCategory")]
        [InlineData("EmployeePanel/AddProduct")]
        [InlineData("EmployeePanel/EditCategory/1")]
        [InlineData("EmployeePanel/EditProduct/1")]
        public async Task Post_EndpointsReturnBadRequestWhenModelStateIsNotValidForAuthenticatedUser(string url)
        {
            // Arrange
            var postRequest = new HttpRequestMessage(HttpMethod.Post, url);

            // Act
            var response = await _clientAuth.SendAsync(postRequest);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
