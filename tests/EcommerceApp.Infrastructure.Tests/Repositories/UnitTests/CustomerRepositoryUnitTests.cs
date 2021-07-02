using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceApp.Domain.Models;
using EcommerceApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EcommerceApp.Infrastructure.Tests.Repositories.UnitTests
{
    public class CustomerRepositoryUnitTests
    {
        private readonly DbContextOptions<AppDbContext> _options;

        public CustomerRepositoryUnitTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        }

        [Fact]
        public async Task GetAllCustomers_GetListOfCustomersAndCheckIfEqualLikeModel()
        {
            //Arrange
            var customer1 = new Customer { Id = 10, FirstName = "unit", LastName = "test", City = "UnitTest", PostalCode = "00-111", Address = "st.Unit" };
            var customer2 = new Customer { Id = 11, FirstName = "units", LastName = "testy", City = "UnitTests", PostalCode = "20-111", Address = "st.Unit 1" };
            var customer3 = new Customer { Id = 12, FirstName = "unitx", LastName = "testz", City = "UnitTestx", PostalCode = "10-111", Address = "st.Unit 2" };
            var customerList = new List<Customer> { customer1, customer2, customer3 };

            using (var context = new AppDbContext(_options))
            {
                //Act
                await context.Database.EnsureCreatedAsync();
                var sut = new CustomerRepository(context);
                await context.Customers.AddRangeAsync(customerList);
                await context.SaveChangesAsync();
                var result = sut.GetAllCustomers();

                //Assert
                Assert.Equal(customerList, result);
            }
        }

        [Fact]
        public async Task GetCustomerIdAsync_FetchCustomerIdAndCheckIfEqualLikeModel()
        {
            //Arrange
            var customer = new Customer { Id = 10, FirstName = "unit", LastName = "test", City = "UnitTest", PostalCode = "00-111", Address = "st.Unit", AppUserId = "xyz123" };

            using (var context = new AppDbContext(_options))
            {
                //Act
                await context.Database.EnsureCreatedAsync();
                var sut = new CustomerRepository(context);
                await context.Customers.AddRangeAsync(customer);
                await context.SaveChangesAsync();
                var result = await sut.GetCustomerIdAsync(customer.AppUserId);

                //Assert
                Assert.Equal(customer.Id, result);
            }
        }

        [Fact]
        public async Task UpdateCustomerAsync_ShouldUpdateCustomerToNewData()
        {
            //Arrange
            var customer = new Customer { Id = 10, FirstName = "unit", LastName = "test", City = "UnitTest", PostalCode = "00-111", Address = "st.Unit" };
            var updatedCustomer = new Customer { Id = 10, FirstName = "new", LastName = "name", City = "sadda", PostalCode = "20-111", Address = "st.Unit 23a" };

            using (var context = new AppDbContext(_options))
            {
                //Act
                await context.Database.EnsureCreatedAsync();
                await context.AddAsync(customer);
                await context.SaveChangesAsync();
            }
            using (var context = new AppDbContext(_options))
            {
                //Act
                await context.Database.EnsureCreatedAsync();
                var sut = new CustomerRepository(context);
                await sut.UpdateCustomerAsync(updatedCustomer);
                await context.SaveChangesAsync();
                var result = await context.Customers.FindAsync(customer.Id);

                //Assert
                Assert.Equal(updatedCustomer.Id, result.Id);
                Assert.Equal(updatedCustomer.FirstName, result.FirstName);
                Assert.Equal(updatedCustomer.LastName, result.LastName);
                Assert.Equal(updatedCustomer.City, result.City);
                Assert.Equal(updatedCustomer.PostalCode, result.PostalCode);
                Assert.Equal(updatedCustomer.Address, result.Address);
            }
        }
    }
}

