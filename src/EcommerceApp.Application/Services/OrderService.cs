using System;
using System.Collections.Generic;
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
        private readonly ICartItemRepository _cartItemRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IImageConverterService _imageConverterService;
        private readonly IMapper _mapper;
        private readonly IPaginationService<OrderForListVM> _orderPaginationService;
        private readonly IPaginationService<CustomerOrderForListVM> _customerOrderPaginationService;

        public OrderService(ICartItemRepository cartItemRepository, ICustomerRepository customerRepository, IOrderRepository orderRepository,
        IProductRepository productRepository, IImageConverterService imageConverterService, IMapper mapper,
        IPaginationService<OrderForListVM> orderPaginationService, IPaginationService<CustomerOrderForListVM> customerOrderPaginationService)
        {
            _cartItemRepository = cartItemRepository;
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _imageConverterService = imageConverterService;
            _mapper = mapper;
            _orderPaginationService = orderPaginationService;
            _customerOrderPaginationService = customerOrderPaginationService;
        }

        public async Task AddOrderAsync(OrderCheckoutVM orderCheckoutVM)
        {
            orderCheckoutVM.CartItems = orderCheckoutVM.CartItems.OrderBy(x => x.ProductId).ToList();
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
            var productIdList = orderCheckoutVM.CartItems.ConvertAll(x => x.ProductId);
            var orderItemList = new List<OrderItem>();
            var productList = await _productRepository.GetAllProducts().Where(x => productIdList.Contains(x.Id)).ToListAsync();
            for (int i = 0; i < orderCheckoutVM.CartItems.Count; i++)
            {
                orderItemList.Add(new OrderItem
                {
                    ProductId = orderCheckoutVM.CartItems[i].ProductId,
                    Quantity = orderCheckoutVM.CartItems[i].Quantity,
                });
                productList[i].UnitsInStock -= orderCheckoutVM.CartItems[i].Quantity;
            }
            order.OrderItems = orderItemList;
            await _orderRepository.AddOrderAsync(order);
            await _productRepository.UpdateProductsAsync(productList);
            await _cartItemRepository.DeleteAllCartItemsByCartIdAsync(orderCheckoutVM.CartId);
        }

        public async Task<OrderCheckoutVM> GetDataForOrderCheckoutAsync(int customerId)
        {
            var orderCheckoutVM = await _customerRepository.GetAllCustomers().Where(x => x.Id == customerId)
            .ProjectTo<OrderCheckoutVM>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
            for (int i = orderCheckoutVM.CartItems.Count - 1; i >= 0; i--)
            {
                if (orderCheckoutVM.CartItems[i].UnitsInStock <= 0)
                {
                    orderCheckoutVM.CartItems.RemoveAt(i);
                }
                else if (orderCheckoutVM.CartItems[i].Quantity > orderCheckoutVM.CartItems[i].UnitsInStock)
                {
                    orderCheckoutVM.CartItems[i].Quantity = orderCheckoutVM.CartItems[i].UnitsInStock;
                    orderCheckoutVM.TotalPrice += orderCheckoutVM.CartItems[i].TotalCartItemPrice;
                    orderCheckoutVM.CartItems[i].ImageUrl = _imageConverterService.GetImageUrlFromByteArray(orderCheckoutVM.CartItems[i].Image);
                }
                else
                {
                    orderCheckoutVM.TotalPrice += orderCheckoutVM.CartItems[i].TotalCartItemPrice;
                    orderCheckoutVM.CartItems[i].ImageUrl = _imageConverterService.GetImageUrlFromByteArray(orderCheckoutVM.CartItems[i].Image);
                }
            }
            return orderCheckoutVM;
        }

        public async Task<ListOrderForListVM> GetAllPaginatedOrdersAsync(int pageSize, int pageNumber)
        {
            var ordersVM = _orderRepository.GetAllOrders().ProjectTo<OrderForListVM>(_mapper.ConfigurationProvider);
            var paginatedVM = await _orderPaginationService.CreateAsync(ordersVM, pageNumber, pageSize);
            return _mapper.Map<ListOrderForListVM>(paginatedVM);
        }

        public async Task<ListCustomerOrderForListVM> GetAllPaginatedCustomerOrdersAsync(int pageSize, int pageNumber, string appUserId)
        {
            var ordersVM = _orderRepository.GetAllOrders().Where(x => x.Customer.AppUserId == appUserId).ProjectTo<CustomerOrderForListVM>(_mapper.ConfigurationProvider);
            var paginatedVM = await _customerOrderPaginationService.CreateAsync(ordersVM, pageNumber, pageSize);
            return _mapper.Map<ListCustomerOrderForListVM>(paginatedVM);
        }

        public async Task<OrderDetailsVM> GetOrderDetailsAsync(int orderId)
        {
            return await _orderRepository.GetAllOrders().Where(x => x.Id == orderId)
            .ProjectTo<OrderDetailsVM>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
        }

        public async Task<CustomerOrderDetailsVM> GetCustomerOrderDetailsAsync(int orderId, string appUserId)
        {
            var customerOrderDetailsVM = await _orderRepository.GetAllOrders().Where(x => x.Id == orderId && x.Customer.AppUserId == appUserId)
            .ProjectTo<CustomerOrderDetailsVM>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
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
