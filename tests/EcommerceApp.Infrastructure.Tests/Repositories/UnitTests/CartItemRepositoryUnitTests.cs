using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Domain.Models;
using EcommerceApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EcommerceApp.Infrastructure.Tests.Repositories.UnitTests
{
    public class CartItemRepositoryUnitTests
    {
        private readonly DbContextOptions<AppDbContext> _options;

        public CartItemRepositoryUnitTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        }

        [Fact]
        public async Task AddCartItemAsync_CheckCartItemExistsAfterAdd()
        {
            //Arrange
            var cartItem = new CartItem { Id = 10, ProductId = 2, Quantity = 3, CartId = 5 };

            using (var context = new AppDbContext(_options))
            {
                //Act
                await context.Database.EnsureCreatedAsync();
                var sut = new CartItemRepository(context);
                await sut.AddCartItemAsync(cartItem);
                var result = await context.CartItems.FindAsync(cartItem.Id);

                //Assert
                Assert.Equal(cartItem, result);
            }
        }

        [Fact]
        public async Task DeleteCartItemAsync_CheckCartItemDoesntExistsAfterDelete()
        {
            //Arrange
            var cartItem1 = new CartItem { Id = 10, ProductId = 2, Quantity = 3, CartId = 5 };
            var cartItem2 = new CartItem { Id = 11, ProductId = 3, Quantity = 2, CartId = 6 };

            using (var context = new AppDbContext(_options))
            {
                //Act
                await context.Database.EnsureCreatedAsync();
                var sut = new CartItemRepository(context);
                await context.CartItems.AddAsync(cartItem1);
                await context.CartItems.AddAsync(cartItem2);
                await context.SaveChangesAsync();
                await sut.DeleteCartItemAsync(cartItem1.Id);
                var result1 = await context.CartItems.FindAsync(cartItem1.Id);
                var result2 = await context.CartItems.FindAsync(cartItem2.Id);

                //Assert
                Assert.Null(result1);
                Assert.Equal(cartItem2, result2);
            }
        }

        [Fact]
        public async Task DeleteAllCartItemsByCartIdAsync_CheckCartItemsDoesntExistsAfterDelete()
        {
            //Arrange
            var cartItem1 = new CartItem { Id = 10, ProductId = 2, Quantity = 3, CartId = 5 };
            var cartItem2 = new CartItem { Id = 11, ProductId = 3, Quantity = 2, CartId = 6 };
            var cartItem3 = new CartItem { Id = 12, ProductId = 3, Quantity = 2, CartId = 6 };
            var cartItemList = new List<CartItem> { cartItem1, cartItem2, cartItem3 };

            using (var context = new AppDbContext(_options))
            {
                //Act
                await context.Database.EnsureCreatedAsync();
                var sut = new CartItemRepository(context);
                await context.CartItems.AddRangeAsync(cartItemList);
                await context.SaveChangesAsync();
                await sut.DeleteAllCartItemsByCartIdAsync(cartItem2.CartId);
                var result1 = await context.CartItems.FindAsync(cartItem1.Id);
                var result2 = await context.CartItems.FindAsync(cartItem2.Id);
                var result3 = await context.CartItems.FindAsync(cartItem3.Id);

                //Assert
                Assert.Equal(cartItem1, result1);
                Assert.Null(result2);
                Assert.Null(result3);
            }
        }

        [Fact]
        public async Task GetAllCartItems_GetListOfCartItemsAndCheckIfEqualLikeModel()
        {
            //Arrange
            var cartItem1 = new CartItem { Id = 10, ProductId = 2, Quantity = 3, CartId = 5 };
            var cartItem2 = new CartItem { Id = 11, ProductId = 3, Quantity = 2, CartId = 6 };
            var cartItem3 = new CartItem { Id = 12, ProductId = 3, Quantity = 2, CartId = 6 };
            var cartItemList = new List<CartItem> { cartItem1, cartItem2, cartItem3 };

            using (var context = new AppDbContext(_options))
            {
                //Act
                await context.Database.EnsureCreatedAsync();
                var sut = new CartItemRepository(context);
                await context.CartItems.AddRangeAsync(cartItemList);
                await context.SaveChangesAsync();
                var result = sut.GetAllCartItems();

                //Assert
                Assert.Equal(cartItemList, result);
            }
        }

        [Fact]
        public async Task GetCartItemAsync_FetchCartItemAndCheckIfEqualLikeModel()
        {
            //Arrange
            var cartItem = new CartItem { Id = 10, ProductId = 2, Quantity = 3, CartId = 5 };

            using (var context = new AppDbContext(_options))
            {
                //Act
                await context.Database.EnsureCreatedAsync();
                var sut = new CartItemRepository(context);
                await context.CartItems.AddRangeAsync(cartItem);
                await context.SaveChangesAsync();
                var result = await sut.GetCartItemAsync(cartItem.Id);

                //Assert
                Assert.Equal(cartItem, result);
            }
        }

        [Fact]
        public async Task UpdateCartItemAsync_ShouldUpdateCartItemToNewData()
        {
            //Arrange
            var cartItem = new CartItem { Id = 10, ProductId = 2, Quantity = 3, CartId = 5 };
            var updatedCartItem = new CartItem { Id = 10, ProductId = 3, Quantity = 5, CartId = 5 };

            using (var context = new AppDbContext(_options))
            {
                //Act
                await context.Database.EnsureCreatedAsync();
                await context.AddAsync(cartItem);
                await context.SaveChangesAsync();
            }
            using (var context = new AppDbContext(_options))
            {
                //Act
                await context.Database.EnsureCreatedAsync();
                var sut = new CartItemRepository(context);
                await sut.UpdateCartItemAsync(updatedCartItem);
                await context.SaveChangesAsync();
                var result = await context.CartItems.FindAsync(cartItem.Id);

                //Assert
                Assert.Equal(updatedCartItem.Id, result.Id);
                Assert.Equal(updatedCartItem.ProductId, result.ProductId);
                Assert.Equal(updatedCartItem.Quantity, result.Quantity);
                Assert.Equal(updatedCartItem.CartId, result.CartId);
            }
        }
    }
}
