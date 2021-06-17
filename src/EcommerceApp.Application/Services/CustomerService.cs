using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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

        public CustomerService(ICustomerRepository customerRepository, IMapper mapper, UserManager<ApplicationUser> userManager, IPaginationService<CustomerVM> paginationService)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _userManager = userManager;
            _paginationService = paginationService;
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
    }
}
