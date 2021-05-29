using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using AutoMapper.QueryableExtensions;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace EcommerceApp.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public EmployeeService(IMapper mapper, IEmployeeRepository employeeRepository, UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _employeeRepository = employeeRepository;
            _userManager = userManager;
        }
        public async Task AddEmployeeAsync(EmployeeVM employeeVM)
        {
            var employee = _mapper.Map<Employee>(employeeVM);
            var user = new ApplicationUser { UserName = employeeVM.Email, Email = employeeVM.Email };
            employee.AppUserId = user.Id;
            var result = await _userManager.CreateAsync(user, employeeVM.Password);

            await _employeeRepository.AddEmployeeAsync(employee);
        }
        public async Task<List<EmployeeVM>> GetAllEmployeesAsync()
        {
            var employees = (await _employeeRepository.GetAllEmployeesAsync()).ToList();
            return _mapper.Map<List<EmployeeVM>>(employees);

        }
        public async Task<EmployeeVM> GetEmployeeAsync(int id)
        {
            var employee = await _employeeRepository.GetEmployeeAsync(id);
            return _mapper.Map<EmployeeVM>(employee);
        }
        public async Task UpdateEmployeeAsync(EmployeeVM employeeVM)
        {
            var employeeToEdit = await _employeeRepository.GetEmployeeAsync(employeeVM.Id);
            var user = await _userManager.FindByIdAsync(employeeToEdit.AppUserId);

            if (employeeVM.Password != null)
            {
                var tokenPassword = await _userManager.GeneratePasswordResetTokenAsync(user);
                await _userManager.ResetPasswordAsync(user, tokenPassword, employeeVM.Password);
            }

            await _userManager.SetEmailAsync(user, employeeVM.Email);
            await _userManager.UpdateNormalizedEmailAsync(user);

            await _userManager.SetUserNameAsync(user, employeeVM.Email);
            await _userManager.UpdateNormalizedUserNameAsync(user);

            var employee = _mapper.Map<Employee>(employeeVM);
            employee.AppUserId = employeeToEdit.AppUserId;
            await _employeeRepository.UpdateEmployeeAsync(employee);
        }
        public async Task DeleteEmployeeAsync(int id)
        {
            var employee = await _employeeRepository.GetEmployeeAsync(id);
            var user = await _userManager.FindByIdAsync(employee.AppUserId);
            await _userManager.DeleteAsync(user);
            await _employeeRepository.DeleteEmployeeAsync(id);
        }

    }
}
