using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPaginationService<EmployeeForListVM> _paginationService;

        public EmployeeService(IMapper mapper, IEmployeeRepository employeeRepository, UserManager<ApplicationUser> userManager,
        IPaginationService<EmployeeForListVM> paginationService, IPasswordHasher<ApplicationUser> passwordHasher)
        {
            _mapper = mapper;
            _employeeRepository = employeeRepository;
            _userManager = userManager;
            _paginationService = paginationService;
            _passwordHasher = passwordHasher;
        }
        public async Task AddEmployeeAsync(EmployeeVM employeeVM)
        {
            var employee = _mapper.Map<Employee>(employeeVM);
            var user = new ApplicationUser { UserName = employeeVM.Email, Email = employeeVM.Email, Employee = employee };
            await _userManager.CreateAsync(user, employeeVM.Password);
            await _userManager.ConfirmEmailAsync(user, await _userManager.GenerateEmailConfirmationTokenAsync(user));
            var claim = new Claim("isEmployee", "True");
            await _userManager.AddClaimAsync(user, claim);
        }

        public async Task<ListEmployeeForListVM> GetAllPaginatedEmployeesAsync(int pageSize, int pageNumber)
        {
            var employeesVM = _employeeRepository.GetAllEmployees().Include(x => x.AppUser).ProjectTo<EmployeeForListVM>(_mapper.ConfigurationProvider);
            var paginatedVM = await _paginationService.CreateAsync(employeesVM, pageNumber, pageSize);
            return _mapper.Map<ListEmployeeForListVM>(paginatedVM);
        }

        public async Task<EmployeeVM> GetEmployeeAsync(int id)
        {
            /*var employee = await _employeeRepository.GetEmployeeAsync(id);
            var employeeVM = _mapper.Map<EmployeeVM>(employee);
            var user = await _userManager.FindByIdAsync(employee.AppUserId);
            employeeVM.Email = user.Email;
            return employeeVM; */
            return await _employeeRepository.GetAllEmployees().Where(x => x.Id == id).Include(a => a.AppUser)
            .ProjectTo<EmployeeVM>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
        }
        public async Task UpdateEmployeeAsync(EmployeeVM employeeVM)
        {
            var employee = await _employeeRepository.GetAllEmployees().Include(x => x.AppUser).FirstOrDefaultAsync(x => x.Id == employeeVM.Id);
            employee.FirstName = employeeVM.FirstName;
            employee.LastName = employeeVM.LastName;
            employee.Position = employeeVM.Position;
            employee.AppUser.UserName = employeeVM.Email;
            employee.AppUser.NormalizedUserName = employeeVM.Email.ToUpper();
            employee.AppUser.Email = employeeVM.Email;
            employee.AppUser.NormalizedEmail = employeeVM.Email.ToUpper();
            if (employeeVM.Password != null)
            {
                employee.AppUser.PasswordHash = _passwordHasher.HashPassword(employee.AppUser, employeeVM.Password);
            }
            await _employeeRepository.UpdateEmployeeAsync(employee);
            /*var employeeToEdit = await _employeeRepository.GetEmployeeAsync(employeeVM.Id);
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
            await _employeeRepository.UpdateEmployeeAsync(employee); */
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
