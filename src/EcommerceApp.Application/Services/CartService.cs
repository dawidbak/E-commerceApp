using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.Cart;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _cartRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly IImageConverterService _imageConverterService;

        public CartService(ICartItemRepository cartItemRepository, IProductRepository productRepository, ICartRepository cartRepository,
        ICustomerRepository customerRepository, IMapper mapper, IImageConverterService imageConverterService)
        {
            _cartItemRepository = cartItemRepository;
            _productRepository = productRepository;
            _cartRepository = cartRepository;
            _customerRepository = customerRepository;
            _mapper = mapper;
            _imageConverterService = imageConverterService;
        }
        public async Task AddCartItemAsync(int productId, int quantity, string appUserId)
        {
            var product = await _productRepository.GetProductAsync(productId);
            var customerId = await _customerRepository.GetCustomerIdAsync(appUserId);
            var cartId = await _cartRepository.GetCartIdAsync(customerId);
            await _cartItemRepository.AddCartItemAsync(new CartItem { Product = product, Quantity = quantity, CartId = cartId });
        }

        public async Task<ListCartItemForListVM> GetAllCartItemsForCurrentUserAsync(string appUserId)
        {
            var customerId = await _customerRepository.GetCustomerIdAsync(appUserId);
            var cartId = await _cartRepository.GetCartIdAsync(customerId);
            var cartItems = await _cartItemRepository.GetAllCartItemsByCartId(cartId).ToListAsync();
            var cartItemList = new List<CartItemForListVM>();
            for (int i = 0; i < cartItems.Count; i++)
            {
                var product = await _productRepository.GetProductAsync(cartItems[i].ProductId);
                cartItemList.Add(new CartItemForListVM
                {
                    Id = cartItems[i].Id,
                    ProductId = product.Id,
                    Name = product.Name,
                    Quantity = cartItems[i].Quantity,
                    ImageUrl = _imageConverterService.GetImageUrlFromByteArray(product.Image),
                    UnitPrice = product.UnitPrice,
                });
            }
            return new ListCartItemForListVM()
            {
                CartItems = cartItemList,
                CartId = cartId,
                CustomerId = customerId
            };
        }

        public async Task<int> GetCartIdAsync(string appUserId)
        {
            var customerId = await _customerRepository.GetCustomerIdAsync(appUserId);
            return await _cartRepository.GetCartIdAsync(customerId);
        }

        public async Task IncreaseQuantityCartItemByOneAsync(int cartItemId)
        {
            var cartItem = await _cartItemRepository.GetCartItemAsync(cartItemId);
            cartItem.Quantity += 1;
            await _cartItemRepository.UpdateCartItemAsync(cartItem);
        }

        public async Task DecreaseQuantityCartItemByOneAsync(int cartItemId)
        {
            var cartItem = await _cartItemRepository.GetCartItemAsync(cartItemId);
            if (cartItem.Quantity > 1)
            {
                cartItem.Quantity -= 1;
            }
            await _cartItemRepository.UpdateCartItemAsync(cartItem);
        }

        public async Task DeleteCartItemAsync(int cartItemId)
        {
            await _cartItemRepository.DeleteCartItemAsync(cartItemId);
        }
    }
}
