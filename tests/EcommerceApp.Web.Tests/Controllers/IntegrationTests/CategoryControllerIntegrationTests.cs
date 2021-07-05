using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace EcommerceApp.Web.Tests.Controllers.IntegrationTests
{
    public class CategoryControllerIntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _sut;
        private readonly HttpClient _client;

        public CategoryControllerIntegrationTests(WebApplicationFactory<Startup> sut)
        {
            _sut = sut;
            _client = _sut.GetGuestHttpClient();
        }

        [Fact]
        public async Task Products_EndpointsReturnSuccessAndCorrectContentType()
        {
            //Act
            var response = await _client.GetAsync("Category/Products?category=test");

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Products_EndpointsReturnNotFound()
        {
            //Act
            var response = await _client.GetAsync("Category/Products?id=1");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
