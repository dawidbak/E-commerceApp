using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace EcommerceApp.Web.Tests.Controllers.IntegrationTests
{
    public class HomeControllerIntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _sut;
        private readonly HttpClient _client;

        public HomeControllerIntegrationTests(WebApplicationFactory<Startup> sut)
        {
            _sut = sut;
            _client = _sut.GetGuestHttpClient();
        }

        [Fact]
        public async Task Index_EndpointReturnSuccessAndCorrectContentType()
        {
            //Act
            var response = await _client.GetAsync("Home/Index");

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }
    }
}
