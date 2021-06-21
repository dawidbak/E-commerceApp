using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EcommerceApp.Application.Interfaces;
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
        private readonly IPaginationService<OrderForListVM> _orderPaginationService;
        private readonly IPaginationService<CustomerOrderForListVM> _customerOrderPaginationService;

        public OrderService(ICartRepository cartRepository, ICartItemRepository cartItemRepository, ICustomerRepository customerRepository,
        IOrderRepository orderRepository, IOrderItemRepository orderItemRepository, UserManager<ApplicationUser> userManager,
        IProductRepository productRepository, IImageConverterService imageConverterService, IMapper mapper, IPaginationService<OrderForListVM> orderPaginationService,
        IPaginationService<CustomerOrderForListVM> customerOrderPaginationService)
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
            _orderPaginationService = orderPaginationService;
            _customerOrderPaginationService = customerOrderPaginationService;
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
                Price = orderCheckoutVM.TotalPrice,
                OrderDate = DateTime.Now
            };
            await _orderRepository.AddOrderAsync(order);
            for (int i = 0; i < orderCheckoutVM.CartItems.Count; i++)
            {
                var orderItem = new OrderItem { ProductId = orderCheckoutVM.CartItems[i].ProductId, Quantity = orderCheckoutVM.CartItems[i].Quantity, OrderId = order.Id };
                await _orderItemRepository.AddOrderItemAsync(orderItem);
            }

            await _cartItemRepository.DeleteAllCartItemsByCartIdAsync(orderCheckoutVM.CartId);
        }

        public async Task<OrderCheckoutVM> GetDataForOrderCheckoutAsync(int customerId)
        {
            var orderCheckoutVM = await _customerRepository.GetAllCustomers().Where(x => x.Id == customerId).Include(x => x.AppUser).Include(x => x.Cart)
            .ThenInclude(y => y.CartItems).ThenInclude(y => y.Product).ProjectTo<OrderCheckoutVM>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
            for (int i = 0; i < orderCheckoutVM.CartItems.Count; i++)
            {
                orderCheckoutVM.TotalPrice += orderCheckoutVM.CartItems[i].TotalCartItemPrice;
                orderCheckoutVM.CartItems[i].ImageUrl = _imageConverterService.GetImageUrlFromByteArray(orderCheckoutVM.CartItems[i].Image);
            }
            return orderCheckoutVM;
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
            var paginatedVM = await _orderPaginationService.CreateAsync(ordersVM, pageNumber, pageSize);
            return new ListOrderForListVM
            {
                Orders = paginatedVM.Items,
                TotalPages = paginatedVM.TotalPages,
                CurrentPage = paginatedVM.CurrentPage
            };
        }

        public async Task<ListCustomerOrderForListVM> GetAllPaginatedCustomerOrdersAsync(int pageSize, int pageNumber, string appUserId)
        {
            var ordersVM = _orderRepository.GetAllOrders().Where(x => x.Customer.AppUserId == appUserId).ProjectTo<CustomerOrderForListVM>(_mapper.ConfigurationProvider);
            var paginatedVM = await _customerOrderPaginationService.CreateAsync(ordersVM, pageNumber, pageSize);
            return _mapper.Map<ListCustomerOrderForListVM>(paginatedVM);
        }

        public async Task<OrderDetailsVM> GetOrderDetailsAsync(int orderId)
        {
            return await _orderRepository.GetAllOrders().Where(x => x.Id == orderId).Include(x => x.OrderItems).ThenInclude(y => y.Product)
            .ProjectTo<OrderDetailsVM>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
        }

        public async Task<CustomerOrderDetailsVM> GetCustomerOrderDetailsAsync(int orderId, string appUserId)
        {
            var customerOrderDetailsVM = await _orderRepository.GetAllOrders().Where(x => x.Id == orderId && x.Customer.AppUserId == appUserId).Include(x => x.OrderItems)
            .ThenInclude(y => y.Product).ProjectTo<CustomerOrderDetailsVM>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
            for (int i = 0; i < customerOrderDetailsVM.OrderItems.Count; i++)
            {
                customerOrderDetailsVM.OrderItems[i].ImageToDisplay = _imageConverterService.GetImageUrlFromByteArray(customerOrderDetailsVM.OrderItems[i].Image);
            }
            return customerOrderDetailsVM;
        }

        public async Task DeleteOrderAsync(int orderId)
        {
            await _orderRepository.DeleteOrderAsync(orderId);
        }
    }
}
