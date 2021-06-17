using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels;
using EcommerceApp.Application.ViewModels.AdminPanel;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPaginationService<EmployeeForListVM> _paginationService;

        public EmployeeService(IMapper mapper, IEmployeeRepository employeeRepository, UserManager<ApplicationUser> userManager, IPaginationService<EmployeeForListVM> paginationService)
        {
            _mapper = mapper;
            _employeeRepository = employeeRepository;
            _userManager = userManager;
            _paginationService = paginationService;
        }
        public async Task AddEmployeeAsync(EmployeeVM employeeVM)
        {
            var employee = _mapper.Map<Employee>(employeeVM);
            var user = new ApplicationUser { UserName = employeeVM.Email, Email = employeeVM.Email };
            employee.AppUserId = user.Id;
            var result = await _userManager.CreateAsync(user, employeeVM.Password);
            await _userManager.ConfirmEmailAsync(user, await _userManager.GenerateEmailConfirmationTokenAsync(user));
            await _employeeRepository.AddEmployeeAsync(employee);

            var claim = new Claim("isEmployee", "True");
            await _userManager.AddClaimAsync(user, claim);
        }
        public async Task<ListEmployeeForListVM> GetAllEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllEmployeesAsync().ToListAsync();
            var employeesVM = _mapper.Map<List<EmployeeForListVM>>(employees);
            for (int i = 0; i < employees.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(employees[i].AppUserId);
                employeesVM[i].Email = user.Email;
            }
            return new ListEmployeeForListVM()
            {
                Employees = employeesVM,
            };
        }

        public async Task<ListEmployeeForListVM> GetAllPaginatedEmployeesAsync(int pageSize, int pageNumber)
        {
            var employees = await _employeeRepository.GetAllEmployeesAsync().ToListAsync();
            var employeesVM = _mapper.Map<List<EmployeeForListVM>>(employees);
            for (int i = 0; i < employees.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(employees[i].AppUserId);
                employeesVM[i].Email = user.Email;
            }
            var paginatedVM = await _paginationService.CreateAsync(employeesVM.AsQueryable(), pageNumber, pageSize);
            return new ListEmployeeForListVM()
            {
                Employees = paginatedVM.Items,
                CurrentPage = paginatedVM.CurrentPage,
                TotalPages = paginatedVM.TotalPages
            };
        }

        public async Task<EmployeeVM> GetEmployeeAsync(int id)
        {
            var employee = await _employeeRepository.GetEmployeeAsync(id);
            var employeeVM = _mapper.Map<EmployeeVM>(employee);
            var user = await _userManager.FindByIdAsync(employee.AppUserId);
            employeeVM.Email = user.Email;
            return employeeVM;
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
            await _employeeRepository.DeleteEmployeeAsync(id);
            await _userManager.DeleteAsync(user);
        }
    }
}
