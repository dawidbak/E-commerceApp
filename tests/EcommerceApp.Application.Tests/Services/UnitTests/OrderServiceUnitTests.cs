using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.Services;
using EcommerceApp.Application.ViewModels;
using EcommerceApp.Application.ViewModels.Cart;
using EcommerceApp.Application.ViewModels.EmployeePanel;
using EcommerceApp.Application.ViewModels.Order;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace EcommerceApp.Application.Tests.Services.UnitTests
{
    public class OrderServiceUnitTests
    {
        private readonly OrderService _sut;
        private readonly Mock<ICartItemRepository> _cartItemRepository = new();
        private readonly Mock<ICustomerRepository> _customerRepository = new();
        private readonly Mock<IOrderRepository> _orderRepository = new();
        private readonly Mock<IProductRepository> _productRepository = new();
        private readonly Mock<IImageConverterService> _imageConverterService = new();
        private readonly Mock<IMapper> _mapper = new();
        private readonly Mock<IPaginationService<OrderForListVM>> _orderPaginationService = new();
        private readonly Mock<IPaginationService<CustomerOrderForListVM>> _customerOrderPaginationService = new();

        public OrderServiceUnitTests()
        {
            _sut = new OrderService(
            _cartItemRepository.Object,
            _customerRepository.Object,
            _orderRepository.Object,
            _productRepository.Object,
            _imageConverterService.Object,
            _mapper.Object,
            _orderPaginationService.Object,
            _customerOrderPaginationService.Object);
        }

        [Fact]
        public async Task AddOrderAsync_ShouldMethodsRunsOnce()
        {
            //Arrange
            var product = new Product
            {
                Id = 1,
                Name = "unit",
                UnitPrice = 9.99m,
                UnitsInStock = 1
            };
            var products = new List<Product> { product };
            var productsQuery = products.AsQueryable().BuildMock();
            var orderCheckoutVM = new OrderCheckoutVM
            {
                CartId = 1,
                TotalPrice = 11.11m,
                CustomerId = 1,
                FirstName = "FirstName",
                LastName = "LastName",
                PostalCode = "12-345",
                Address = "st. Unit",
                City = "Test",
                Email = "test@test.com",
                PhoneNumber = "1234567789",
                CartItems = new List<CartItemForListVM>
                {
                    new CartItemForListVM
                    {
                        ProductId = 1,
                        Name = "unit",
                        Quantity = 1
                    }
                }
            };

            _productRepository.Setup(x => x.GetAllProducts()).Returns(productsQuery.Object);

            //Act
            await _sut.AddOrderAsync(orderCheckoutVM);

            //Assert
            _orderRepository.Verify(x => x.AddOrderAsync(It.IsAny<Order>()), Times.Once);
            _productRepository.Verify(x => x.UpdateProductsAsync(It.IsAny<List<Product>>()), Times.Once);
            _cartItemRepository.Verify(x => x.DeleteAllCartItemsByCartIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetDataForOrderCheckoutAsync_ReturnsOrderCheckoutVMAndCheckIfEqualToModel()
        {
            var customer = new Customer
            {
                Id = 1,
                AppUser = new ApplicationUser
                {
                    Email = "test@test.com",
                    PhoneNumber = "123456678"
                },
                Cart = new Cart
                {
                    Id = 1,
                    CartItems = new List<CartItem>
                    {
                        new CartItem
                        {
                            Id= 1,
                            Quantity = 2,
                            Product = new Product
                            {
                                Id = 1,
                                Name = "unit",
                                UnitPrice = 11.23m,
                                UnitsInStock = 10,
                                Image = new byte[]{1,2}
                            }
                        }
                    }
                }
            };
            var customers = new List<Customer> { customer };
            var customersQuery = customers.AsQueryable().BuildMock();

            var cartItems = customer.Cart.CartItems.ToList();

            var orderCheckoutVM = new OrderCheckoutVM
            {
                CustomerId = customer.Id,
                CartId = customer.Cart.Id,
                Email = customer.AppUser.Email,
                PhoneNumber = customer.AppUser.PhoneNumber,
                CartItems = new List<CartItemForListVM>
                {
                    new CartItemForListVM
                    {
                        Id = cartItems[0].Id,
                        ProductId = cartItems[0].Product.Id,
                        Name = cartItems[0].Product.Name,
                        UnitPrice = cartItems[0].Product.UnitPrice,
                        UnitsInStock = cartItems[0].Product.UnitsInStock,
                        Quantity = cartItems[0].Quantity,
                        ImageUrl = "image",
                        Image = cartItems[0].Product.Image
                    }
                },
            };

            _mapper.Setup(x => x.ConfigurationProvider).Returns(() => new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Customer, OrderCheckoutVM>()
                .ForMember(x => x.CartItems, y => y.MapFrom(src => src.Cart.CartItems))
                .ForMember(x => x.CustomerId, y => y.MapFrom(src => src.Id))
                .ForMember(x => x.CartId, y => y.MapFrom(src => src.Cart.Id))
                .ForMember(x => x.Email, y => y.MapFrom(src => src.AppUser.Email))
                .ForMember(x => x.PhoneNumber, y => y.MapFrom(src => src.AppUser.PhoneNumber));
                cfg.CreateMap<CartItem, CartItemForListVM>();
            }));
            _customerRepository.Setup(x => x.GetAllCustomers()).Returns(customersQuery.Object);
            _imageConverterService.Setup(x => x.GetImageUrlFromByteArray(orderCheckoutVM.CartItems[0].Image)).Returns(orderCheckoutVM.CartItems[0].ImageUrl);

            //Act
            var result = await _sut.GetDataForOrderCheckoutAsync(1);

            //Assert
            Assert.Equal(orderCheckoutVM.CustomerId, result.CustomerId);
            Assert.Equal(orderCheckoutVM.FirstName, result.FirstName);
            Assert.Equal(orderCheckoutVM.LastName, result.LastName);
        }

        [Fact]
        public async Task GetAllPaginatedOrdersAsync_ReturnsListOrderForListVMAndCheckIfEqualToModel()
        {
            //Arrange
            var orders = new List<Order>
            {
                new Order{Id = 1, Price = 12},
                new Order{Id = 2,Price = 23}
            };
            var orderForListVM = new List<OrderForListVM>
            {
                new OrderForListVM{Id = 1, Price = 12},
                new OrderForListVM{Id = 2, Price = 23},
            };
            PaginatedVM<OrderForListVM> paginatedVM = new()
            {
                Items = orderForListVM,
                CurrentPage = 1,
                TotalPages = 2,
            };
            ListOrderForListVM listOrderForListVM = new()
            {
                Orders = orderForListVM,
                CurrentPage = 1,
                TotalPages = 2,
            };

            _mapper.Setup(x => x.ConfigurationProvider).Returns(() => new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Order, OrderForListVM>();
            }));
            _orderRepository.Setup(x => x.GetAllOrders()).Returns(orders.AsQueryable());
            _orderPaginationService.Setup(x => x.CreateAsync(It.IsAny<IQueryable<OrderForListVM>>(), 1, 10)).ReturnsAsync(paginatedVM);
            _mapper.Setup(x => x.Map<ListOrderForListVM>(paginatedVM)).Returns(listOrderForListVM);

            //Act
            var result = await _sut.GetAllPaginatedOrdersAsync(10, 1);

            //Assert
            Assert.Equal(listOrderForListVM, result);
        }

        [Fact]
        public async Task GetAllPaginatedCustomerOrdersAsync_ReturnsListCustomerOrderForListVMAndCheckIfEqualToModel()
        {
            //Arrange
            var customer = new Customer { AppUserId = "sdadasd" };
            var orders = new List<Order>
            {
                new Order{Id = 1, Price = 12, Customer = customer},
                new Order{Id = 2,Price = 23, Customer = customer}
            };
            var customerOrderForListVM = new List<CustomerOrderForListVM>
            {
                new CustomerOrderForListVM{Id = 1, Price = 12},
                new CustomerOrderForListVM{Id = 2, Price = 23},
            };
            PaginatedVM<CustomerOrderForListVM> paginatedVM = new()
            {
                Items = customerOrderForListVM,
                CurrentPage = 1,
                TotalPages = 2,
            };
            ListCustomerOrderForListVM listCustomerOrderForListVM = new()
            {
                Orders = customerOrderForListVM,
                CurrentPage = 1,
                TotalPages = 2,
            };

            _mapper.Setup(x => x.ConfigurationProvider).Returns(() => new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Order, CustomerOrderForListVM>();
            }));
            _orderRepository.Setup(x => x.GetAllOrders()).Returns(orders.AsQueryable());
            _customerOrderPaginationService.Setup(x => x.CreateAsync(It.IsAny<IQueryable<CustomerOrderForListVM>>(), 1, 10)).ReturnsAsync(paginatedVM);
            _mapper.Setup(x => x.Map<ListCustomerOrderForListVM>(paginatedVM)).Returns(listCustomerOrderForListVM);

            //Act
            var result = await _sut.GetAllPaginatedCustomerOrdersAsync(10, 1, customer.AppUserId);

            //Assert
            Assert.Equal(listCustomerOrderForListVM, result);
        }

        [Fact]
        public async Task GetCustomerOrderDetailsAsync_ReturnsCustomerOrderDetailsVMAndCheckIfEqualToModel()
        {
            //Arrange
            var customer = new Customer { AppUserId = "sdadasd" };
            var product = new Product { Id = 10, Name = "asdasdas", UnitsInStock = 2, UnitPrice = 3.23m };
            var orderItems = new List<OrderItem>
            {
                new OrderItem{Id = 1, Product = product}
            };
            var orders = new List<Order>
            {
                new Order{Id = 1, Price = 12, Customer = customer, OrderItems = orderItems, ShipFirstName = "test", ShipLastName = "Unit" },
            };
            var customerOrderDetailsVM = new CustomerOrderDetailsVM
            {
                Id = 1,
                ShipFirstName = "test",
                ShipLastName = "Unit",
                OrderItems = new List<OrderItemForCustomerOrderDetailsVM> { new OrderItemForCustomerOrderDetailsVM { ImageToDisplay = "sdads" } }
            };
            var ordersQuery = orders.AsQueryable().BuildMock();

            _mapper.Setup(x => x.ConfigurationProvider).Returns(() => new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Order, CustomerOrderDetailsVM>();
                cfg.CreateMap<OrderItem, OrderItemForCustomerOrderDetailsVM>()
                .ForMember(x => x.ProductName, y => y.MapFrom(src => src.Product.Name))
                .ForMember(x => x.Image, y => y.MapFrom(src => src.Product.Image))
                .ForMember(x => x.UnitPrice, y => y.MapFrom(src => src.Product.UnitPrice));
            }));
            _orderRepository.Setup(x => x.GetAllOrders()).Returns(ordersQuery.Object);
            _imageConverterService.Setup(x => x.GetImageUrlFromByteArray(It.IsAny<byte[]>())).Returns(customerOrderDetailsVM.OrderItems[0].ImageToDisplay);

            //Act
            var result = await _sut.GetCustomerOrderDetailsAsync(1, customer.AppUserId);

            //Assert
            Assert.Equal(customerOrderDetailsVM.Id, result.Id);
            Assert.Equal(customerOrderDetailsVM.ShipFirstName, result.ShipFirstName);
            Assert.Equal(customerOrderDetailsVM.ShipLastName, result.ShipLastName);
        }

        [Fact]
        public async Task GetOrderDetailsAsync_ReturnsOrderDetailsVMAndCheckIfEqualToModel()
        {
            //Arrange
            var customer = new Customer { AppUserId = "sdadasd" };
            var product = new Product { Id = 10, Name = "asdasdas", UnitsInStock = 2, UnitPrice = 3.23m };
            var orderItems = new List<OrderItem>
            {
                new OrderItem{Id = 1, Product = product}
            };
            var orderItemsForDetailsVM = new List<OrderItemsForDetailsVM>
            {
                new OrderItemsForDetailsVM{ProductId = product.Id, ProductName = product.Name}
            };
            var orders = new List<Order>
            {
                new Order
                {
                    Id = 1,
                    Price = 12,
                    Customer = customer,
                    OrderItems = orderItems,
                    ShipFirstName = "test",
                    ShipLastName = "Unit"
                },
            };
            var ordersQuery = orders.AsQueryable().BuildMock();
            var orderDetailsVM = new OrderDetailsVM
            {
                Id = 1,
                Price = 12,
                ShipFirstName = "test",
                ShipLastName = "Unit",
                OrderItems = orderItemsForDetailsVM
            };

            _mapper.Setup(x => x.ConfigurationProvider).Returns(() => new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Order, OrderDetailsVM>();
                cfg.CreateMap<OrderItem, OrderItemsForDetailsVM>();
            }));
            _orderRepository.Setup(x => x.GetAllOrders()).Returns(ordersQuery.Object);

            //Act
            var result = await _sut.GetOrderDetailsAsync(1);

            //Assert
            Assert.Equal(orderDetailsVM.Id, result.Id);
            Assert.Equal(orderDetailsVM.Price, result.Price);
            Assert.Equal(orderDetailsVM.ShipFirstName, result.ShipFirstName);
            Assert.Equal(orderDetailsVM.ShipLastName, result.ShipLastName);
        }

        [Fact]
        public async Task DeleteOrderAsync_ShouldDeleteMethodRunsOnce()
        {
            //Arrange
            var order = new Order { Id = 1 };

            //Act
            await _sut.DeleteOrderAsync(order.Id);

            //Assert
            _orderRepository.Verify(x => x.DeleteOrderAsync(It.IsAny<int>()), Times.Once);
        }
    }
}
