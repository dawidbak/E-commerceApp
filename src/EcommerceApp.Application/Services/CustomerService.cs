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

        public CustomerService(ICustomerRepository customerRepository, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task DeleteCustomerAsync(int customerId)
        {
            var customer = await _customerRepository.GetCustomerAsync(customerId);
            var user = await _userManager.FindByIdAsync(customer.AppUserId);
            await _userManager.DeleteAsync(user);
        }

        public async Task<ListCustomerVM> GetAllCustomersAsync()
        {
            var customers = (await _customerRepository.GetAllCustomersAsync()).ToList();
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
