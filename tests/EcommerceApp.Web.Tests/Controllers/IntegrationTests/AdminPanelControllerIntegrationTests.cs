using System;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using EcommerceApp.Application.ViewModels;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using EcommerceApp.Web.Tests.Controllers.Helpers;
using EcommerceApp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace EcommerceApp.Web.Tests.Controllers.IntegrationTests
{
    public class AdminPanelControllerIntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _sut;
        private readonly HttpClient _client;

        public AdminPanelControllerIntegrationTests(WebApplicationFactory<Startup> sut)
        {
            //Arrange
            _sut = sut;
            _client = _sut.WithWebHostBuilder(builder =>
           {
               builder.ConfigureTestServices(services =>
               {
                   services.AddAuthentication("Test").AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });

                   var descriptor = services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<AppDbContext>));

                   if (descriptor != null)
                   {
                       services.Remove(descriptor);
                   }

                   var serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                   services.AddDbContext<AppDbContext>(options =>
                   {
                       options.UseInMemoryDatabase("InMemoryAdminPanelTest");
                       options.UseInternalServiceProvider(serviceProvider);
                   });

                   var sp = services.BuildServiceProvider();

                   using (var scope = sp.CreateScope())
                   {
                       using (var context = scope.ServiceProvider.GetRequiredService<AppDbContext>())
                       {
                           try
                           {
                               context.Database.EnsureCreated();
                           }
                           catch (Exception ex)
                           {
                               throw new(ex.Message);
                           }
                       }
                   }
               });
           }).CreateClient(new WebApplicationFactoryClientOptions
           {
               AllowAutoRedirect = false
           });
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

        }

        [Fact]
        public async Task Index_ReturnsListOfEmployeesVM()
        {
            //Act
            var response = await _client.GetAsync("/AdminPanel");
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Contains("<th>Id</th>", content);
            Assert.Contains("<th>Email</th>", content);
            Assert.Contains("<th>First Name</th>", content);
            Assert.Contains("<th>Last Name</th>", content);
            Assert.Contains("<th>Position</th>", content);
        }
    }
}
