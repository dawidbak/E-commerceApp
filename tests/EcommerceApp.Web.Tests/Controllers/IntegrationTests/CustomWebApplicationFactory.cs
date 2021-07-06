using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using EcommerceApp.Domain.Models;
using EcommerceApp.Infrastructure;
using EcommerceApp.Web.Tests.Controllers.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EcommerceApp.Web.Tests.Controllers.IntegrationTests
{
    public static class CustomWebApplicationFactory
    {
        #region AdminPanelHttpClient
        public static HttpClient GetAdminPanelHttpClient(this WebApplicationFactory<Startup> webApplicationFactory)
        {
            return webApplicationFactory.WithWebHostBuilder(builder =>
           {
               builder.ConfigureTestServices(services =>
               {
                   services.AddAuthentication("Test").AddScheme<AuthenticationSchemeOptions, AdminTestAuthHandler>("Test", options => { });

                   var descriptor = services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<AppDbContext>));

                   if (descriptor != null)
                   {
                       services.Remove(descriptor);
                   }

                   var serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                   services.AddDbContext<AppDbContext>(options =>
                   {
                       options.UseInMemoryDatabase("AdminPanelIntegrationTest");
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
                                    Id = 1,
                                    FirstName = "Test",
                                    LastName = "Last",
                                    Position = "Position",
                                    AppUserId = "123test",
                                });
                               context.Users.Add(new ApplicationUser
                               {
                                   Id = "123test",
                                   Email = "test@example.com",
                                   Customer = new Customer
                                   {
                                       Id = 1,
                                       FirstName = "test",
                                       LastName = "integration",
                                       Cart = new Cart
                                       {
                                           CartItems = new List<CartItem>
                                           {
                                               new CartItem
                                               {
                                                   Quantity = 5
                                               }
                                           }
                                       }
                                   }
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
        }
        #endregion

        #region EmployeeHttpClient
        public static HttpClient GetEmployeeHttpClient(this WebApplicationFactory<Startup> webApplicationFactory)
        {
            return webApplicationFactory.WithWebHostBuilder(builder =>
           {
               builder.ConfigureTestServices(services =>
               {
                   services.AddAuthentication("Test").AddScheme<AuthenticationSchemeOptions, EmployeeTestAuthHandler>("Test", options => { });

                   var descriptor = services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<AppDbContext>));

                   if (descriptor != null)
                   {
                       services.Remove(descriptor);
                   }

                   var serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                   services.AddDbContext<AppDbContext>(options =>
                   {
                       options.UseInMemoryDatabase("EmployeePanelIntegrationTest");
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
                               context.Orders.Add(
                                   new Order
                                   {
                                       Id = 1,
                                       OrderItems = new List<OrderItem>
                                       {
                                           new OrderItem
                                           {
                                                Product = new Product
                                                {
                                                    Id = 1,
                                                    Image = new byte[] { 23, 4 },
                                                    Category = new Category
                                                    {
                                                        Id = 1,
                                                        Image = new byte[] { 2, 2, 3 }
                                                    }
                                                }
                                           }
                                       }
                                   }
                               );
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
        }
        #endregion

        #region CustomerHttpClient
        public static HttpClient GetCustomerHttpClient(this WebApplicationFactory<Startup> webApplicationFactory)
        {
            return webApplicationFactory.WithWebHostBuilder(builder =>
           {
               builder.ConfigureTestServices(services =>
               {
                   services.AddAuthentication("Test").AddScheme<AuthenticationSchemeOptions, CustomerTestAuthHandler>("Test", options => { });

                   var descriptor = services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<AppDbContext>));

                   if (descriptor != null)
                   {
                       services.Remove(descriptor);
                   }

                   var serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                   services.AddDbContext<AppDbContext>(options =>
                   {
                       options.UseInMemoryDatabase("CustomersIntegrationTests");
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
                               context.Add(new ApplicationUser
                               {
                                   Id = "customer",
                                   Email = "test@example.com",
                                   Customer = new Customer
                                   {
                                       Id = 1,
                                       AppUserId = "customer",
                                       FirstName = "test",
                                       LastName = "integration",
                                       Cart = new Cart
                                       {
                                           Id = 1,
                                           CartItems = new List<CartItem>
                                           {
                                               new CartItem
                                               {
                                                   Id = 1,
                                                   Quantity = 5,
                                                   Product = new Product
                                                   {
                                                       Id = 1,
                                                       Name = "test",
                                                       UnitPrice = 1,
                                                       Image = new byte[]{1,2},
                                                       UnitsInStock = 10,
                                                   }
                                               }
                                           }
                                       },
                                       Orders = new List<Order>
                                       {
                                           new Order
                                           {
                                               ShipFirstName = "test"
                                           }
                                       }
                                   }
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
        }
        #endregion
        #region GuestHttpClient
        public static HttpClient GetGuestHttpClient(this WebApplicationFactory<Startup> webApplicationFactory)
        {
            return webApplicationFactory.WithWebHostBuilder(builder =>
           {
               builder.ConfigureTestServices(services =>
               {
                   var descriptor = services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<AppDbContext>));

                   if (descriptor != null)
                   {
                       services.Remove(descriptor);
                   }

                   var serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                   services.AddDbContext<AppDbContext>(options =>
                   {
                       options.UseInMemoryDatabase("GuestIntegrationTests");
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
                               context.Add(new Category
                               {
                                   Id = 1,
                                   Name = "test",
                                   Products = new List<Product>
                                   {
                                       new Product
                                       {
                                            Id = 1,
                                            Name = "test",
                                            UnitPrice = 1,
                                            Image = new byte[]{1,2},
                                            UnitsInStock = 10,
                                       }
                                   }
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
        }
        #endregion
    }

}
