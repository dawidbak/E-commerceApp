using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.Cart;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _cartRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CartService(ICartItemRepository cartItemRepository, IProductRepository productRepository, ICartRepository cartRepository,
        ICustomerRepository customerRepository, IMapper mapper)
        {
            _cartItemRepository = cartItemRepository;
            _productRepository = productRepository;
            _cartRepository = cartRepository;
            _customerRepository = customerRepository;
            _mapper = mapper;
        }
        public async Task AddCartItemAsync(int productId, int quantity, string appUserId)
        {
            var product = await _productRepository.GetProductAsync(productId);
            var customerId = await _customerRepository.GetCustomerIdAsync(appUserId);
            var cartId = await _cartRepository.GetCartId(customerId);
            await _cartItemRepository.AddCartItemAsync(new CartItem { Product = product, Quantity = quantity, CartId = cartId });
        }

        public async Task<ListCartItemForListVM> GetAllCartItemsForCurrentUser(string appUserId)
        {
            var customerId = await _customerRepository.GetCustomerIdAsync(appUserId);
            var cartId = await _cartRepository.GetCartId(customerId);
            var cartItems = (await _cartItemRepository.GetAllCartItemsAsync(cartId)).ToList();

            var productIdList = new List<int>();
            for (int i = 0; i < cartItems.Count; i++)
            {
                productIdList.Add(cartItems[i].ProductId);
            }

            List<Product> productList = new();
            foreach (var id in productIdList)
            {
                var product = await _productRepository.GetProductAsync(id);
                productList.Add(product);
            }

            var cartItemsVM = _mapper.Map<List<CartItemForListVM>>(productList);

            for (int i = 0; i < cartItemsVM.Count; i++)
            {
                cartItemsVM[i].Quantity = cartItems[i].Quantity;
                cartItemsVM[i].TotalPrice = cartItemsVM[i].Quantity * cartItemsVM[i].UnitPrice;
            }
            return new ListCartItemForListVM
            {
                CartItems = cartItemsVM
            };
        }
    }
}
