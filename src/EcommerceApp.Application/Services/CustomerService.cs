using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.AdminPanel;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPaginationService<CustomerVM> _paginationService;
        private readonly IImageConverterService _imageConverterService;
        private readonly IOrderRepository _orderRepository;
        private readonly ICartItemRepository _cartItemRepository;

        public CustomerService(ICustomerRepository customerRepository, IMapper mapper, UserManager<ApplicationUser> userManager, IPaginationService<CustomerVM> paginationService,
        IImageConverterService imageConverterService, IOrderRepository orderRepository, ICartItemRepository cartItemRepository)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _userManager = userManager;
            _paginationService = paginationService;
            _imageConverterService = imageConverterService;
            _orderRepository = orderRepository;
            _cartItemRepository = cartItemRepository;
        }

        public async Task DeleteCustomerAsync(int customerId)
        {
            var customer = await _customerRepository.GetCustomerAsync(customerId);
            var user = await _userManager.FindByIdAsync(customer.AppUserId);
            await _userManager.DeleteAsync(user);
        }

        public async Task<int> GetCustomerIdByAppUserIdAsync(string appUserId)
        {
            return await _customerRepository.GetCustomerIdAsync(appUserId);
        }

        public async Task<ListCustomerVM> GetAllPaginatedCustomersAsync(int pageSize, int pageNumber)
        {
            var customersVM = _customerRepository.GetAllCustomers().Include(x => x.AppUser).ProjectTo<CustomerVM>(_mapper.ConfigurationProvider);
            var paginatedVM = await _paginationService.CreateAsync(customersVM, pageNumber, pageSize);
            return _mapper.Map<ListCustomerVM>(paginatedVM);
        }

        public async Task<CustomerDetailsVM> GetCustomerDetailsAsync(int customerId)
        {
            var customerDetailsVM = await _customerRepository.GetAllCustomers().Where(x => x.Id == customerId).Include(x => x.AppUser).ProjectTo<CustomerDetailsVM>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
            var ordersVM = await _orderRepository.GetAllOrders().Where(x => x.CustomerId == customerId).OrderByDescending(x => x.Id).Take(2).ProjectTo<OrderForCustomerDetailsVM>(_mapper.ConfigurationProvider).ToListAsync();
            var cartItemsVM = await _cartItemRepository.GetAllCartItems().Where(x => x.Cart.CustomerId == customerId).Include(x => x.Product).ProjectTo<CartItemForCustomerDetailsVM>(_mapper.ConfigurationProvider).ToListAsync();
            customerDetailsVM.Orders = ordersVM;
            customerDetailsVM.CartItems = cartItemsVM;
            for (int i = 0; i < customerDetailsVM.CartItems.Count; i++)
            {
                customerDetailsVM.CartItems[i].ImageUrl = _imageConverterService.GetImageUrlFromByteArray(customerDetailsVM.CartItems[i].Image);
            }
            return customerDetailsVM;
        }
    }
}
