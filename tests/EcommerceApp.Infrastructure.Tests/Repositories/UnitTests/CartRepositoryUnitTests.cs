using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceApp.Domain.Models;
using EcommerceApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EcommerceApp.Infrastructure.Tests.Repositories.UnitTests
{
    public class CartRepositoryUnitTests
    {
        private readonly DbContextOptions<AppDbContext> _options;

        public CartRepositoryUnitTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        }

        [Fact]
        public async Task AddCartAsync_CheckCartExistsAfterAdd()
        {
            //Arrange
            var cart = new Cart { Id = 1, CustomerId = 2 };

            using (var context = new AppDbContext(_options))
            {
                //Act
                await context.Database.EnsureCreatedAsync();
                var sut = new CartRepository(context);
                await sut.AddCartAsync(cart);
                var result = await context.Carts.FindAsync(cart.Id);

                //Assert
                Assert.Equal(cart, result);
            }
        }

        [Fact]
        public async Task GetAllCarts_GetListOfCartsAndCheckIfEqualLikeModel()
        {
            //Arrange
            var cart1 = new Cart { Id = 1, CustomerId = 2 };
            var cart2 = new Cart { Id = 2, CustomerId = 23 };
            var cart3 = new Cart { Id = 3, CustomerId = 2 };
            var cartList = new List<Cart> { cart1, cart2, cart3 };

            using (var context = new AppDbContext(_options))
            {
                //Act
                await context.Database.EnsureCreatedAsync();
                var sut = new CartRepository(context);
                await context.Carts.AddRangeAsync(cartList);
                await context.SaveChangesAsync();
                var result = sut.GetAllCarts();

                //Assert
                Assert.Equal(cartList, result);
            }
        }
    }
}
