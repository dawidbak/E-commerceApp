using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.Cart;
using EcommerceApp.Application.ViewModels.Order;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace EcommerceApp.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IImageConverterService _imageConverterService;

        public OrderService(ICartRepository cartRepository, ICartItemRepository cartItemRepository, ICustomerRepository customerRepository,
        IOrderRepository orderRepository, IOrderItemRepository orderItemRepository, UserManager<ApplicationUser> userManager,
        IProductRepository productRepository, IImageConverterService imageConverterService)
        {
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _userManager = userManager;
            _productRepository = productRepository;
            _imageConverterService = imageConverterService;
        }

        public async Task AddOrderAsync(OrderCheckoutVM orderCheckoutVM)
        {
            var order = new Order
            {
                CustomerId = orderCheckoutVM.CustomerId,
                ShipFirstName = orderCheckoutVM.FirstName,
                ShipLastName = orderCheckoutVM.LastName,
                ShipAddress = orderCheckoutVM.Address,
                ShipCity = orderCheckoutVM.City,
                ShipContactEmail = orderCheckoutVM.Email,
                ShipContactPhone = orderCheckoutVM.PhoneNumber,
                ShipPostalCode = orderCheckoutVM.PostalCode,
                Price = orderCheckoutVM.TotalPrice
            };
            await _orderRepository.AddOrderAsync(order);
            for (int i = 0; i < orderCheckoutVM.CartItems.Count; i++)
            {
                var orderItem = new OrderItem { ProductId = orderCheckoutVM.CartItems[i].ProductId, Quantity = orderCheckoutVM.CartItems[i].Quantity, OrderId = order.Id };
                await _orderItemRepository.AddOrderItemAsync(orderItem);
            }

            await _cartItemRepository.DeleteAllCartItemsByCartIdAsync(orderCheckoutVM.CartId);
        }

        public async Task<OrderCheckoutVM> GetDataForOrderCheckoutAsync(int cartId)
        {
            var cart = await _cartRepository.GetCartAsync(cartId);
            var customer = await _customerRepository.GetCustomerAsync(cart.CustomerId);
            var appUser = await _userManager.FindByIdAsync(customer.AppUserId);
            var cartItems = (await _cartItemRepository.GetAllCartItemsByCartIdAsync(cartId)).ToList();
            var cartItemList = new List<CartItemForListVM>();
            var totalPrice = 0m;
            for (int i = 0; i < cartItems.Count; i++)
            {
                var product = await _productRepository.GetProductAsync(cartItems[i].ProductId);
                cartItemList.Add(new CartItemForListVM
                {
                    Id = cartItems[i].Id,
                    ProductId = product.Id,
                    Name = product.Name,
                    UnitPrice = product.UnitPrice,
                    Quantity = cartItems[i].Quantity,
                    ImageUrl = _imageConverterService.GetImageUrlFromByteArray(product.Image),
                });
                totalPrice += product.UnitPrice * cartItems[i].Quantity;
            }
            return new OrderCheckoutVM
            {
                CartItems = cartItemList,
                CartId = cartId,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                CustomerId = customer.Id,
                TotalPrice = totalPrice,
                Email = appUser.Email,
                PhoneNumber = appUser.PhoneNumber,
                City = customer.City,
                PostalCode = customer.PostalCode,
                Address = customer.Address,
            };
        }
    }
}
