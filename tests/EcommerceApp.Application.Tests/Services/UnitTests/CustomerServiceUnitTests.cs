using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.Services;
using EcommerceApp.Application.ViewModels;
using EcommerceApp.Application.ViewModels.AdminPanel;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Microsoft.AspNetCore.Identity;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace EcommerceApp.Application.Tests.Services.UnitTests
{
    public class CustomerServiceUnitTests
    {
        private readonly CustomerService _sut;
        private readonly Mock<ICustomerRepository> _customerRepository = new();
        private readonly Mock<IMapper> _mapper = new();
        private readonly Mock<UserManager<ApplicationUser>> _userManager;
        private readonly Mock<IPaginationService<CustomerVM>> _paginationService = new();
        private readonly Mock<IImageConverterService> _imageConverterService = new();
        private readonly Mock<IOrderRepository> _orderRepository = new();
        private readonly Mock<ICartItemRepository> _cartItemRepository = new();

        public CustomerServiceUnitTests()
        {
            var mockUserStore = new Mock<IUserStore<ApplicationUser>>();
            _userManager = new Mock<UserManager<ApplicationUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);
            _sut = new CustomerService(
                _customerRepository.Object,
                _mapper.Object, _userManager.Object,
                _paginationService.Object,
                _imageConverterService.Object,
                _orderRepository.Object,
                _cartItemRepository.Object);
        }

        [Fact]
        public async Task DeleteCustomerAsync_DeleteMethodShouldRunsOnce()
        {
            //Arrange
            var customer = new Customer { Id = 1, AppUserId = "xyz123" };
            var customers = new List<Customer>() { customer };
            var customersQuery = customers.AsQueryable().BuildMock();

            _customerRepository.Setup(x => x.GetAllCustomers()).Returns(customersQuery.Object);

            //Act
            await _sut.DeleteCustomerAsync(customer.Id);

            //Arrange
            _userManager.Verify(x => x.DeleteAsync(It.IsAny<ApplicationUser>()), Times.Once);
        }

        [Fact]
        public async Task GetCustomerIdByAppUserIdAsync_ReturnsCustomerIdAndCheckIfEqualToModel()
        {
            //Arrange
            var appUser = new ApplicationUser { Id = "123" };
            var customer = new Customer { Id = 10 };

            _customerRepository.Setup(x => x.GetCustomerIdAsync(appUser.Id)).ReturnsAsync(customer.Id);

            //Act
            var result = await _sut.GetCustomerIdByAppUserIdAsync(appUser.Id);

            //Assert
            Assert.Equal(customer.Id, result);
        }

        [Fact]
        public async Task GetAllPaginatedCustomersAsync_ReturnsListCustomerVMAndCheckIfEqualToModel()
        {
            var customers = new List<Customer>()
            {
                new Customer{Id = 1,FirstName = "Unit", LastName = "Test"},
                new Customer{Id=2,FirstName = "Uasdasnit", LastName= "Tesasdsat"},
            };
            var customerVMs = new List<CustomerVM>()
            {
                new CustomerVM{Id =1,FirstName = "Unit",LastName ="Test"},
                new CustomerVM{Id =2,FirstName = "Uasdasnit", LastName= "Tesasdsat"},
            };
            PaginatedVM<CustomerVM> paginatedVM = new()
            {
                Items = customerVMs,
                CurrentPage = 1,
                TotalPages = 2,
            };
            var listCustomerVM = new ListCustomerVM
            {
                Customers = customerVMs,
                CurrentPage = 1,
                TotalPages = 2,
            };
            _mapper.Setup(x => x.ConfigurationProvider).Returns(() => new MapperConfiguration(cfg =>
            { cfg.CreateMap<Customer, CustomerVM>(); }));

            _customerRepository.Setup(x => x.GetAllCustomers()).Returns(customers.AsQueryable());
            _paginationService.Setup(x => x.CreateAsync(It.IsAny<IQueryable<CustomerVM>>(), 1, 10)).ReturnsAsync(paginatedVM);
            _mapper.Setup(x => x.Map<ListCustomerVM>(paginatedVM)).Returns(listCustomerVM);

            //Act
            var result = await _sut.GetAllPaginatedCustomersAsync(10, 1);

            //Assert
            Assert.Equal(listCustomerVM, result);
        }

        [Fact]
        public async Task GetCustomerDetailsAsync_ReturnsCustomerDetailsVMAndCheckIfEqualToModel()
        {
            //Arrange
            var product = new Product { Name = "asda", Id = 1, UnitPrice = 3.23m };
            var orders = new List<Order>()
            {
                new Order{Id = 1}
            };
            var orderForCustomerDetailsVMs = new List<OrderForCustomerDetailsVM>()
            {
                new OrderForCustomerDetailsVM{Id = 1},
            };
            var cartItems = new List<CartItem>()
            {
                new CartItem{ProductId = 1,Product = product,Quantity = 2, Cart = new Cart{Id = 1, CustomerId = 1}}
            };
            var cartItemForCustomerDetailsVMs = new List<CartItemForCustomerDetailsVM>()
            {
                new CartItemForCustomerDetailsVM{ProductId = 1, ProductName ="asda",Quantity = 2},
            };
            var customers = new List<Customer>()
            {
                new Customer{Id = 1, FirstName = "sadasd", LastName = "sdsda", AppUser = new ApplicationUser
                {
                    Email = "sdsad@asp.com"
                }, Orders = orders}
            };
            var customerDetailsVM = new CustomerDetailsVM
            {
                Id = 1,
                FirstName = "sadasd",
                LastName = "sdsda",
                Orders = orderForCustomerDetailsVMs,
                CartItems = cartItemForCustomerDetailsVMs
            };
            var customersQuery = customers.AsQueryable().BuildMock();
            var ordersQuery = orders.AsQueryable().BuildMock();
            var cartItemsQuery = cartItems.AsQueryable().BuildMock();
            _mapper.Setup(x => x.ConfigurationProvider).Returns(() => new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Order, OrderForCustomerDetailsVM>();
                cfg.CreateMap<CartItem, CartItemForCustomerDetailsVM>();
                cfg.CreateMap<Customer, CustomerDetailsVM>();
            }));

            _customerRepository.Setup(x => x.GetAllCustomers()).Returns(customersQuery.Object);
            _orderRepository.Setup(x => x.GetAllOrders()).Returns(ordersQuery.Object);
            _cartItemRepository.Setup(x => x.GetAllCartItems()).Returns(cartItemsQuery.Object);

            //Act
            var result = await _sut.GetCustomerDetailsAsync(1);

            //
            Assert.Equal(customerDetailsVM.Id, result.Id);
            Assert.Equal(customerDetailsVM.FirstName, result.FirstName);
            Assert.Equal(customerDetailsVM.LastName, result.LastName);
            Assert.Equal(customerDetailsVM.Email,result.Email);
        }

    }
}
