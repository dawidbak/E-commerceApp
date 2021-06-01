using System.Net;
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
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Web.Tests.Controllers.IntegrationTests
{
    public class AdminPanelControllerIntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _sut;
        private readonly HttpClient _client;
        private readonly HttpClient _clientUnAuthorized;

        public AdminPanelControllerIntegrationTests(WebApplicationFactory<Startup> sut)
        {
            //Arrange
            int IdEmployee = 1;
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
                               context.Employees.Add(
                                new Employee
                                {
                                    Id = IdEmployee,
                                    FirstName = "Test",
                                    LastName = "Last",
                                    Position = "Position",
                                    AppUserId = "123test",
                                    Email = "test@example.com"
                                });
                               context.Users.Add(new ApplicationUser
                               {
                                   Id = "123test",
                                   Email = "test@example.com",
                               });
                               context.SaveChanges();
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

            _clientUnAuthorized = _sut.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Theory]
        [InlineData("/AdminPanel")]
        [InlineData("/AdminPanel/AddEmployee")]
        [InlineData("/AdminPanel/EditEmployee/1")]
        [InlineData("/AdminPanel/DeleteEmployee/1")]
        public async Task AllActions_ShouldRedirectUnauthorizedUser(string url)
        {
            //Act
            var response = await _clientUnAuthorized.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.StartsWith("http://localhost/Identity/Account/Login", response.Headers.Location.OriginalString);
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


        [Fact]
        public async Task AddEmployee_Get_ReturnsAddEmployeeForm()
        {
            //Act
            var response = await _client.GetAsync("/AdminPanel/AddEmployee");
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Contains($"<form action=\"/AdminPanel/AddEmployee\" method=\"post\">", content);
        }

        [Fact]
        public async Task AddEmployee_Post_SentGoodModelAndReturnsRedirect()
        {
            //Arrange
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/AdminPanel/AddEmployee");
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
            var response = await _client.SendAsync(postRequest);

            //Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        }

        [Fact]
        public async Task AddEmployee_Post_SentBadModelAndShouldntRedirect()
        {
            //Arrange
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/AdminPanel/AddEmployee");
            var employeeVM = new Dictionary<string, string>
            {
                {"Email" , "test"},
                {"Password" ,"Pa$$w0rd!"},
                {"FirstName" , "I"},
                {"LastName" , "Test"},
                {"Position", "Employee"}
            };
            postRequest.Content = new FormUrlEncodedContent(employeeVM);

            //Act
            var response = await _client.SendAsync(postRequest);

            //Assert
            Assert.NotEqual(HttpStatusCode.Redirect, response.StatusCode);
        }

        [Fact]
        public async Task EditEmployee_Get_ReturnsEditEmployeeForm()
        {
            //Act
            var response = await _client.GetAsync($"/AdminPanel/EditEmployee/1");
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Contains("<form action=\"/AdminPanel/EditEmployee/1\" method=\"post\">", content);
        }

        [Fact]
        public async Task EditEmployee_Get_ReturnsNotFoundStatusCode()
        {
            //Act
            var response = await _client.GetAsync($"/AdminPanel/EditEmployee/dw");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task EditEmployee_Post_SentValidModelAndShouldRedirect()
        {
            //Arrange
            var employeeVM = new Dictionary<string, string>
            {
                {"Email" , "test@example.com"},
                {"Password" ,"Pa$$w0rd!"},
                {"FirstName" , "Integration"},
                {"LastName" , "Test"},
                {"Position", "Employee"}
            };
            var content = new FormUrlEncodedContent(employeeVM);

            //Act
            var response = await _client.PostAsync("AdminPanel/EditEmployee/1", content);

            //Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        }

        [Fact]
        public async Task EditEmployee_Post_SentBadModelAndShouldntRedirect()
        {
            //Arrange
            var employeeVM = new Dictionary<string, string>
            {
                {"Email" , "te"},
                {"Password" ,"Pa$$w0rd!"},
                {"FirstName" , "I"},
                {"LastName" , "T"},
                {"Position", "E"}
            };
            var content = new FormUrlEncodedContent(employeeVM);

            //Act
            var response = await _client.PostAsync("AdminPanel/EditEmployee/1", content);

            //Assert
            Assert.NotEqual(HttpStatusCode.Redirect, response.StatusCode);
        }

        [Fact]
        public async Task DeleteEmployee_ShouldDeleteEmployeeAndRedirect()
        {
            //Act
            var response = await _client.GetAsync("AdminPanel/DeleteEmployee/1");

            //Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        }

        [Fact]
        public async Task DeleteEmployee_UsedWrongParameterAndShouldNotFound()
        {
            //Act
            var response = await _client.GetAsync("AdminPanel/DeleteEmployee/one");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}

