using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.AccessControl;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.Services;
using EcommerceApp.Application.ViewModels.Cart;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Core;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace EcommerceApp.Application.Tests.Services.UnitTests
{
    public class CartServiceUnitTests
    {
        private readonly CartService _sut;
        private readonly Mock<ICartItemRepository> _cartItemRepository = new();
        private readonly Mock<IProductRepository> _productRepository = new();
        private readonly Mock<ICartRepository> _cartRepository = new();
        private readonly Mock<IMapper> _mapper = new();
        private readonly Mock<IImageConverterService> _imageConverterService = new();

        public CartServiceUnitTests()
        {
            _sut = new CartService(_cartItemRepository.Object, _productRepository.Object, _cartRepository.Object, _mapper.Object, _imageConverterService.Object);
        }

        [Fact]
        public async Task AddCartItemAsync_ShouldCartItemAddMethodRunsOnce()
        {
            //Arrange
            var product = new Product { Id = 1, Name = "product", Description = "xyz", UnitsInStock = 10, UnitPrice = 12.50m };
            var customer = new Customer { Id = 1, AppUserId = "xyz123" };
            var carts = new List<Cart>
            {
                new Cart{Id = 1, CustomerId = customer.Id,Customer = customer},
            };
            var cart = new Cart { Id = 1, CustomerId = 2 };
            var cartItem = new CartItem { Id = 1, Product = product, CartId = 1, Quantity = 5 };
            var cartsQuery = carts.AsQueryable().BuildMock();
            _productRepository.Setup(x => x.GetProductAsync(product.Id)).ReturnsAsync(product);
            _cartRepository.Setup(x => x.GetAllCarts()).Returns(cartsQuery.Object);

            //Act
            await _sut.AddCartItemAsync(product.Id, 5, "xyz123");

            //Assert
            _cartItemRepository.Verify(x => x.AddCartItemAsync(It.IsAny<CartItem>()), Times.Once);
        }

        [Fact]
        public async Task GetAllCartItemsForCurrentUserAsync_ShouldReturnsListCartItemForListVMAndCheckIfEqualLikeModel()
        {
            //Arrange
            var customer = new Customer { Id = 1, AppUserId = "xyz123", AppUser = new ApplicationUser { Id = "sadasd" } };
            var product = new Product { Id = 1, Name = "product", Description = "xyz", UnitsInStock = 10, UnitPrice = 12.50m };
            var cartItem = new CartItem { Id = 1, Product = product, CartId = 1, Quantity = 5 };
            var cartItems = new List<CartItem>() { cartItem };
            var cartItemForListVM = new List<CartItemForListVM>()
            {
                new CartItemForListVM{Id = 1,Quantity = 2,ProductId = cartItem.ProductId},
            };
            var carts = new List<Cart>()
            {
                new Cart{Id = 1, CustomerId = customer.Id, Customer = customer, CartItems = cartItems },
            };
            var listCartItemFortListVM = new ListCartItemForListVM { CustomerId = customer.Id, CartItems = cartItemForListVM, TotalPrice = 2 };

            _mapper.Setup(x => x.ConfigurationProvider).Returns(() => new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Cart, ListCartItemForListVM>();
                cfg.CreateMap<CartItem, CartItemForListVM>();
            }));
            var cartsQuery = carts.AsQueryable().BuildMock();
            _cartRepository.Setup(x => x.GetAllCarts()).Returns(cartsQuery.Object);


            var result = await _sut.GetAllCartItemsForCurrentUserAsync(customer.AppUserId);

            //Assert
            Assert.Equal(listCartItemFortListVM.CustomerId, result.CustomerId);
            Assert.Equal(listCartItemFortListVM.CartItems[0].Id, result.CartItems[0].Id);
            Assert.Equal(listCartItemFortListVM.CartItems[0].ProductId, result.CartItems[0].ProductId);
        }

        [Fact]
        public async Task IncreaseQuantityCartItemByOneAsync_ShouldUpdateCartItemMethodRunsOnce()
        {
            //Arrange
            var product = new Product { Id = 1, Name = "product", Description = "xyz", UnitsInStock = 10, UnitPrice = 12.50m, Image = new byte[] { 1, 2 } };
            var cartItem = new CartItem { Id = 1, Product = product, CartId = 1, Quantity = 5 };
            var cartItems = new List<CartItem>() { cartItem };
            var cartItemsQuery = cartItems.AsQueryable().BuildMock();

            _cartItemRepository.Setup(x => x.GetAllCartItems()).Returns(cartItemsQuery.Object);

            //Act
            await _sut.IncreaseQuantityCartItemByOneAsync(cartItem.Id);

            //Assert
            _cartItemRepository.Verify(x => x.UpdateCartItemAsync(It.IsAny<CartItem>()), Times.Once);
        }

        [Fact]
        public async Task DecreaseQuantityCartItemByOneAsync_ShouldUpdateCartItemMethodRunsOnce()
        {
            //Arrange
            var product = new Product { Id = 1, Name = "product", Description = "xyz", UnitsInStock = 10, UnitPrice = 12.50m, Image = new byte[] { 1, 2 } };
            var cartItem = new CartItem { Id = 1, Product = product, CartId = 1, Quantity = 5 };

            _cartItemRepository.Setup(x => x.GetCartItemAsync(cartItem.Id)).ReturnsAsync(cartItem);

            //Act
            await _sut.DecreaseQuantityCartItemByOneAsync(cartItem.Id);

            //Assert
            _cartItemRepository.Verify(x => x.UpdateCartItemAsync(It.IsAny<CartItem>()), Times.Once);
        }

        [Fact]
        public async Task DeleteCategoryAsync_ShouldDeleteMethodRunsOnce()
        {
            //Arrange
            var cartItem = new CartItem { Id = 1, CartId = 1, Quantity = 5 };

            //Act
            await _sut.DeleteCartItemAsync(cartItem.Id);

            //Assert
            _cartItemRepository.Verify(x => x.DeleteCartItemAsync(It.IsAny<int>()), Times.Once);
        }
    }
}
