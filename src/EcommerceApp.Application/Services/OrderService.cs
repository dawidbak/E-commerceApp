using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.Cart;
using EcommerceApp.Application.ViewModels.EmployeePanel;
using EcommerceApp.Application.ViewModels.Order;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
        private readonly IMapper _mapper;
        private readonly IPaginationService<OrderForListVM> _paginationService;

        public OrderService(ICartRepository cartRepository, ICartItemRepository cartItemRepository, ICustomerRepository customerRepository,
        IOrderRepository orderRepository, IOrderItemRepository orderItemRepository, UserManager<ApplicationUser> userManager,
        IProductRepository productRepository, IImageConverterService imageConverterService, IMapper mapper, IPaginationService<OrderForListVM> paginationService)
        {
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _userManager = userManager;
            _productRepository = productRepository;
            _imageConverterService = imageConverterService;
            _mapper = mapper;
            _paginationService = paginationService;
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
            var cartItems = await _cartItemRepository.GetAllCartItemsByCartId(cartId).ToListAsync();
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

        public async Task<ListOrderForListVM> GetAllOrdersAsync()
        {
            var ordersVM = await _orderRepository.GetAllOrders().ProjectTo<OrderForListVM>(_mapper.ConfigurationProvider).ToListAsync();
            return new ListOrderForListVM
            {
                Orders = ordersVM
            };
        }

        public async Task<ListOrderForListVM> GetAllPaginatedOrdersAsync(int pageSize, int pageNumber)
        {
            var ordersVM = _orderRepository.GetAllOrders().ProjectTo<OrderForListVM>(_mapper.ConfigurationProvider);
            var paginatedVM = await _paginationService.CreateAsync(ordersVM, pageNumber, pageSize);
            return new ListOrderForListVM
            {
                Orders = paginatedVM.Items,
                TotalPages = paginatedVM.TotalPages,
                CurrentPage = paginatedVM.CurrentPage
            };
        }

        public async Task<OrderDetailsVM> GetOrderDetailsAsync(int orderId)
        {
            var result = await _orderRepository.GetAllOrders().Where(x => x.Id == orderId).Include(x => x.OrderItems).ThenInclude(y => y.Product).FirstOrDefaultAsync();
            var orderItemsVM = _mapper.Map<List<OrderItemsForDetailsVM>>(result.OrderItems);
            var orderDetailsVM = _mapper.Map<OrderDetailsVM>(result);
            orderDetailsVM.OrderItems = orderItemsVM;
            return orderDetailsVM;
        }
    }
}
