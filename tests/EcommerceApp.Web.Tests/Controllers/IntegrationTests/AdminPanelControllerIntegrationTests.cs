using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace EcommerceApp.Web.Tests.Controllers.IntegrationTests
{

    public class AdminPanelControllerIntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _sut;
        private readonly HttpClient _clientAuth;
        private readonly HttpClient _clientUnauth;

        public AdminPanelControllerIntegrationTests(WebApplicationFactory<Startup> sut)
        {
            //Arrange
            _sut = sut;

            _clientAuth = _sut.GetAdminPanelHttpClient();

            _clientUnauth = _sut.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Theory]
        [InlineData("/AdminPanel/Index")]
        [InlineData("/AdminPanel/Employees")]
        [InlineData("/AdminPanel/Customers")]
        [InlineData("/AdminPanel/CustomerDetails")]
        [InlineData("/AdminPanel/AddEmployee")]
        [InlineData("/AdminPanel/EditEmployee/1")]
        [InlineData("/AdminPanel/DeleteEmployee/1")]
        [InlineData("/AdminPanel/DeleteCustomer/1")]
        public async Task AllActions_ReturnsRedirectUnauthorizedUser(string url)
        {
            //Act
            var response = await _clientUnauth.GetAsync(url);

            //Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.StartsWith("http://localhost/Identity/Account/Login", response.Headers.Location.OriginalString);
        }

        [Theory]
        [InlineData("/AdminPanel/Index")]
        [InlineData("/AdminPanel/Employees")]
        [InlineData("/AdminPanel/Customers")]
        [InlineData("/AdminPanel/CustomerDetails/1")]
        [InlineData("/AdminPanel/AddEmployee")]
        [InlineData("/AdminPanel/EditEmployee/1")]
        [InlineData("/AdminPanel/Employees?PageSize=10&SelectedValue=FirstName&SearchString=Test")]
        [InlineData("/AdminPanel/Customers?PageSize=10&SelectedValue=FirstName&SearchString=Test")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentTypeForAuthenticatedUser(string url)
        {
            //Act
            var response = await _clientAuth.GetAsync(url);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData("/AdminPanel/CustomerDetails")]
        [InlineData("/AdminPanel/EditEmployee")]
        [InlineData("/AdminPanel/DeleteEmployee")]
        [InlineData("/AdminPanel/DeleteCustomer")]
        public async Task Get_EndpointsReturnNotFoundForAuthenticatedUser(string url)
        {
            //Act
            var response = await _clientAuth.GetAsync(url);

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData("/AdminPanel/AddEmployee")]
        [InlineData("/AdminPanel/EditEmployee/1")]
        public async Task Post_EndpointsReturnRedirectWhenModelStateIsValidForAuthenticatedUser(string url)
        {
            //Arrange
            var postRequest = new HttpRequestMessage(HttpMethod.Post, url);
            var employeeVM = new Dictionary<string, string>
            {
                {"Email" , "test@example.com"},
                {"Password" ,"Pa$$w0rd!"},
                {"FirstName" , "Integration"},
                {"LastName" , "Test"},
                {"Position", "Employee"}
            };
            postRequest.Content = new FormUrlEncodedContent(employeeVM);

            //Act
            var response = await _clientAuth.SendAsync(postRequest);

            //Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/AdminPanel/Employees", response.Headers.Location.OriginalString);
        }

        [Theory]
        [InlineData("/AdminPanel/AddEmployee")]
        [InlineData("/AdminPanel/EditEmployee/1")]
        public async Task Post_EndpointsReturnBadRequestForAuthenticatedUser(string url)
        {
            //Arrange
            var postRequest = new HttpRequestMessage(HttpMethod.Post, url);
            var employeeVM = new Dictionary<string, string>
            {

            };
            postRequest.Content = new FormUrlEncodedContent(employeeVM);

            //Act
            var response = await _clientAuth.SendAsync(postRequest);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task DeleteEmployee_EndpointReturnRedirectAndCorrectLocationForAuthenticatedUser()
        {
            //Act
            var response = await _clientAuth.GetAsync("/AdminPanel/DeleteEmployee/1");

            //Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/AdminPanel/Employees", response.Headers.Location.OriginalString);
        }

        [Fact]
        public async Task DeleteCustomer_EndpointReturnRedirectAndCorrectLocationForAuthenticatedUser()
        {
            //Act
            var response = await _clientAuth.GetAsync("/AdminPanel/DeleteCustomer/1");

            //Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/AdminPanel/Customers", response.Headers.Location.OriginalString);
        }
    }
}

