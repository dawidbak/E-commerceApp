using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceApp.Domain.Models;
using EcommerceApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EcommerceApp.Infrastructure.Tests.Repositories.UnitTests
{
    public class OrderRepositoryUnitTests
    {
        private readonly DbContextOptions<AppDbContext> _options;

        public OrderRepositoryUnitTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        }

        [Fact]
        public async Task AddOrderAsync_CheckOrderExistsAfterAdd()
        {
            //Arrange
            var order = new Order
            {
                Id = 10,
                Price = 2.52m,
                ShipFirstName = "Unit",
                ShipLastName = "Test",
                ShipContactEmail = "unit@test.com",
                ShipContactPhone = "123-456-789",
                ShipCity = "City",
                ShipPostalCode = "12-345",
                ShipAddress = "st. Unit 23a",
                OrderDate = DateTime.Parse("2020-01-01")
            };

            using (var context = new AppDbContext(_options))
            {
                //Act
                await context.Database.EnsureCreatedAsync();
                var sut = new OrderRepository(context);
                await sut.AddOrderAsync(order);
                var result = await context.Orders.FindAsync(order.Id);

                //Assert
                Assert.Equal(order, result);
            }
        }

        [Fact]
        public async Task DeleteOrderAsync_CheckOrderDoesntExistsAfterDelete()
        {
            //Arrange
            var order1 = new Order
            {
                Id = 10,
                Price = 2.52m,
                ShipFirstName = "Unit",
                ShipLastName = "Test",
                ShipContactEmail = "unit@test.com",
                ShipContactPhone = "123-456-789",
                ShipCity = "City",
                ShipPostalCode = "12-345",
                ShipAddress = "st. Unit 23a",
                OrderDate = DateTime.Parse("2020-01-01")
            };
            var order2 = new Order
            {
                Id = 11,
                Price = 3.52m,
                ShipFirstName = "Unist",
                ShipLastName = "Tesst",
                ShipContactEmail = "unit@tesst.com",
                ShipContactPhone = "223-456-789",
                ShipCity = "Cityx",
                ShipPostalCode = "12-342",
                ShipAddress = "st. Unit 13",
                OrderDate = DateTime.Parse("2020-01-21")
            };

            using (var context = new AppDbContext(_options))
            {
                //Act
                await context.Database.EnsureCreatedAsync();
                var sut = new OrderRepository(context);
                await context.Orders.AddAsync(order1);
                await context.Orders.AddAsync(order2);
                await context.SaveChangesAsync();
                await sut.DeleteOrderAsync(order1.Id);
                var result1 = await context.Orders.FindAsync(order1.Id);
                var result2 = await context.Orders.FindAsync(order2.Id);

                //Assert
                Assert.Null(result1);
                Assert.Equal(order2, result2);
            }
        }

        [Fact]
        public async Task GetAllOrders_GetListOfOrdersAndCheckIfEqualLikeModel()
        {
            //Arrange
            var order1 = new Order
            {
                Id = 10,
                Price = 2.52m,
                ShipFirstName = "Unit",
                ShipLastName = "Test",
                ShipContactEmail = "unit@test.com",
                ShipContactPhone = "123-456-789",
                ShipCity = "City",
                ShipPostalCode = "12-345",
                ShipAddress = "st. Unit 23a",
                OrderDate = DateTime.Parse("2020-01-01")
            };
            var order2 = new Order
            {
                Id = 11,
                Price = 3.52m,
                ShipFirstName = "Unist",
                ShipLastName = "Tesst",
                ShipContactEmail = "unit@tesst.com",
                ShipContactPhone = "223-456-789",
                ShipCity = "Cityx",
                ShipPostalCode = "12-342",
                ShipAddress = "st. Unit 13",
                OrderDate = DateTime.Parse("2020-01-21")
            };
            var order3 = new Order
            {
                Id = 12,
                Price = 13.52m,
                ShipFirstName = "Unists",
                ShipLastName = "Tessst",
                ShipContactEmail = "usnit@tesst.com",
                ShipContactPhone = "523-456-789",
                ShipCity = "Cixtyx",
                ShipPostalCode = "32-342",
                ShipAddress = "st. Unit a13",
                OrderDate = DateTime.Parse("2020-01-18")
            };
            var orderList = new List<Order> { order1, order2, order3 };

            using (var context = new AppDbContext(_options))
            {
                //Act
                await context.Database.EnsureCreatedAsync();
                var sut = new OrderRepository(context);
                await context.Orders.AddRangeAsync(orderList);
                await context.SaveChangesAsync();
                var result = sut.GetAllOrders();

                //Assert
                Assert.Equal(orderList, result);
            }
        }
    }
}
