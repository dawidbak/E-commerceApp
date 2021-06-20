using System.Collections.Generic;
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

        public CustomerService(ICustomerRepository customerRepository, IMapper mapper, UserManager<ApplicationUser> userManager, IPaginationService<CustomerVM> paginationService,
        IImageConverterService imageConverterService)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _userManager = userManager;
            _paginationService = paginationService;
            _imageConverterService = imageConverterService;
        }

        public async Task DeleteCustomerAsync(int customerId)
        {
            var customer = await _customerRepository.GetCustomerAsync(customerId);
            var user = await _userManager.FindByIdAsync(customer.AppUserId);
            await _userManager.DeleteAsync(user);
        }

        public async Task<ListCustomerVM> GetAllCustomersAsync()
        {
            var customers = await _customerRepository.GetAllCustomers().ToListAsync();
            var customersVM = _mapper.Map<List<CustomerVM>>(customers);
            for (int i = 0; i < customers.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(customers[i].AppUserId);
                customersVM[i].Email = user.Email;
                customersVM[i].PhoneNumber = user.PhoneNumber;
            }
            return new ListCustomerVM()
            {
                Customers = customersVM
            };
        }

        public async Task<int> GetCustomerIdByAppUserIdAsync(string appUserId)
        {
            return await _customerRepository.GetCustomerIdAsync(appUserId);
        }

        public async Task<ListCustomerVM> GetAllPaginatedCustomersAsync(int pageSize, int pageNumber)
        {
            var customers = await _customerRepository.GetAllCustomers().ToListAsync();
            var customersVM = _mapper.Map<List<CustomerVM>>(customers);
            for (int i = 0; i < customers.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(customers[i].AppUserId);
                customersVM[i].Email = user.Email;
                customersVM[i].PhoneNumber = user.PhoneNumber;
            }
            var paginatedVM = await _paginationService.CreateAsync(customersVM.AsQueryable(), pageNumber, pageSize);
            return new ListCustomerVM()
            {
                Customers = paginatedVM.Items,
                TotalPages = paginatedVM.TotalPages,
                CurrentPage = paginatedVM.CurrentPage
            };
        }

        public async Task<CustomerVM> GetCustomerAsync(int customerId)
        {
            var customer = await _customerRepository.GetCustomerAsync(customerId);
            var customerVM = _mapper.Map<CustomerVM>(customer);

            var user = new ApplicationUser { Id = customer.AppUserId };
            customerVM.Email = await _userManager.GetEmailAsync(user);
            customerVM.PhoneNumber = await _userManager.GetEmailAsync(user);
            return customerVM;
        }

        public async Task<CustomerDetailsVM> GetCustomerDetailsAsync(int customerId)
        {
            var customerDetailsVM = await _customerRepository.GetAllCustomers().Where(x => x.Id == customerId).Include(x => x.Orders).Include(x => x.Cart)
            .ThenInclude(y => y.CartItems).ThenInclude(y => y.Product).ProjectTo<CustomerDetailsVM>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
            for (int i = 0; i < customerDetailsVM.CartItems.Count; i++)
            {
                customerDetailsVM.CartItems[i].ImageUrl = _imageConverterService.GetImageUrlFromByteArray(customerDetailsVM.CartItems[i].Image);
            }
            return customerDetailsVM;
        }
    }
}
