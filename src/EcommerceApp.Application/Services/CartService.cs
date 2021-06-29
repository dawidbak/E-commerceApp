using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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
        private readonly IMapper _mapper;
        private readonly IImageConverterService _imageConverterService;

        public CartService(ICartItemRepository cartItemRepository, IProductRepository productRepository, ICartRepository cartRepository,
        IMapper mapper, IImageConverterService imageConverterService)
        {
            _cartItemRepository = cartItemRepository;
            _productRepository = productRepository;
            _cartRepository = cartRepository;
            _mapper = mapper;
            _imageConverterService = imageConverterService;
        }
        public async Task AddCartItemAsync(int productId, int quantity, string appUserId)
        {
            var product = await _productRepository.GetProductAsync(productId);
            if (product.UnitsInStock > 0 && product.UnitsInStock >= quantity)
            {
                var cart = await _cartRepository.GetAllCarts().FirstOrDefaultAsync(x => x.Customer.AppUserId == appUserId);
                await _cartItemRepository.AddCartItemAsync(new CartItem { Product = product, Quantity = quantity, CartId = cart.Id });
            }
        }

        public async Task<ListCartItemForListVM> GetAllCartItemsForCurrentUserAsync(string appUserId)
        {
            var cartItemListVM = await _cartRepository.GetAllCarts().Where(x => x.Customer.AppUserId == appUserId)
                .ProjectTo<ListCartItemForListVM>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
            for (int i = 0; i < cartItemListVM.CartItems.Count; i++)
            {
                cartItemListVM.CartItems[i].ImageUrl = _imageConverterService.GetImageUrlFromByteArray(cartItemListVM.CartItems[i].Image);
                cartItemListVM.TotalPrice += cartItemListVM.CartItems[i].TotalCartItemPrice;
            }
            return cartItemListVM;
        }

        public async Task IncreaseQuantityCartItemByOneAsync(int cartItemId)
        {
            var cartItem = await _cartItemRepository.GetAllCartItems().Include(p => p.Product).FirstOrDefaultAsync(x => x.Id == cartItemId);
            if (cartItem.Quantity >= cartItem.Product.UnitsInStock)
            {
                cartItem.Quantity = cartItem.Product.UnitsInStock;
            }
            else
            {
                cartItem.Quantity++;
            }
            await _cartItemRepository.UpdateCartItemAsync(cartItem);
        }

        public async Task DecreaseQuantityCartItemByOneAsync(int cartItemId)
        {
            var cartItem = await _cartItemRepository.GetCartItemAsync(cartItemId);
            if (cartItem.Quantity <= 1)
            {
                cartItem.Quantity = 1;
            }
            else
            {
                cartItem.Quantity--;
            }
            await _cartItemRepository.UpdateCartItemAsync(cartItem);
        }

        public async Task DeleteCartItemAsync(int cartItemId)
        {
            await _cartItemRepository.DeleteCartItemAsync(cartItemId);
        }
    }
}
