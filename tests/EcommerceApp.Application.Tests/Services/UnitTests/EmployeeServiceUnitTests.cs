using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
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
    public class EmployeeServiceUnitTests
    {
        private readonly EmployeeService _sut;
        private readonly Mock<IMapper> _mapper = new();
        private readonly Mock<IEmployeeRepository> _employeeRepository = new();
        private readonly Mock<UserManager<ApplicationUser>> _userManager;
        private readonly Mock<IPasswordHasher<ApplicationUser>> _passwordHasher = new();
        private readonly Mock<IPaginationService<EmployeeForListVM>> _paginationService = new();

        public EmployeeServiceUnitTests()
        {
            var mockUserStore = new Mock<IUserStore<ApplicationUser>>();
            _userManager = new Mock<UserManager<ApplicationUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);
            _sut = new EmployeeService(_mapper.Object, _employeeRepository.Object, _userManager.Object, _paginationService.Object, _passwordHasher.Object);
        }

        [Fact]
        public async Task AddEmployee_ShouldEmployeeExistsAfterAdd()
        {
            //Arrange
            var employee = new Employee() { FirstName = "unit", LastName = "test", Position = "xunit" };
            var employeeVM = new EmployeeVM() { FirstName = "unit", LastName = "test", Position = "xunit", Email = "test@test.com" };

            _mapper.Setup(x => x.Map<Employee>(employeeVM)).Returns(employee);
            _userManager.Setup(x => x.GenerateEmailConfirmationTokenAsync(It.IsAny<ApplicationUser>())).ReturnsAsync("token");

            //Act
            await _sut.AddEmployeeAsync(employeeVM);

            //Assert
            _userManager.Verify(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
            _userManager.Verify(x => x.AddClaimAsync(It.IsAny<ApplicationUser>(), It.IsAny<Claim>()), Times.Once);
            _userManager.Verify(x => x.ConfirmEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task GetEmployeeAsync_ShouldReturnsEmployeeVMAndCheckIfEqualToModel()
        {
            //Arrange
            var employee = new Employee() { Id = 11, FirstName = "unit", LastName = "test", Position = "xunit" };
            var employeeVM = new EmployeeVM() { Id = 11, FirstName = "unit", LastName = "test", Position = "xunit" };

            var employees = new List<Employee> { employee };
            var employeesQuery = employees.AsQueryable().BuildMock();

            _mapper.Setup(x => x.ConfigurationProvider).Returns(() => new MapperConfiguration(cfg =>
            { cfg.CreateMap<Employee, EmployeeVM>(); }));
            _employeeRepository.Setup(x => x.GetAllEmployees()).Returns(employeesQuery.Object);

            //Act
            var result = await _sut.GetEmployeeAsync(employee.Id);

            //Assert
            Assert.Equal(employeeVM.Id, result.Id);
            Assert.Equal(employeeVM.FirstName, result.FirstName);
            Assert.Equal(employeeVM.LastName, result.LastName);
            Assert.Equal(employeeVM.Position, result.Position);
        }

        [Fact]
        public async Task GetAllPaginatedEmployeesAsync_ReturnsListEmployeeForListVMAndCheckIfEqualToModel()
        {
            //Arrange
            var employee1 = new Employee() { Id = 12, FirstName = "unit", LastName = "test", Position = "xunit" };
            var employee2 = new Employee() { Id = 13, FirstName = "unit1", LastName = "test1", Position = "xunit1" };
            List<Employee> employees = new() { employee1, employee2 };
            var employeeVM1 = new EmployeeForListVM() { FirstName = "unit", LastName = "test", Position = "xunit" };
            var employeeVM2 = new EmployeeForListVM() { FirstName = "unit1", LastName = "test1", Position = "xunit1" };
            List<EmployeeForListVM> employeeForListVMs = new() { employeeVM1, employeeVM2 };

            PaginatedVM<EmployeeForListVM> paginatedVM = new()
            {
                Items = employeeForListVMs,
                CurrentPage = 1,
                TotalPages = 2,
            };
            var listEmployeeForListVM = new ListEmployeeForListVM()
            {
                Employees = employeeForListVMs,
                CurrentPage = 1,
                TotalPages = 2,
            };

            _mapper.Setup(x => x.ConfigurationProvider).Returns(() => new MapperConfiguration(cfg =>
            { cfg.CreateMap<Employee, EmployeeForListVM>(); }));

            _employeeRepository.Setup(x => x.GetAllEmployees()).Returns(employees.AsQueryable);
            _paginationService.Setup(x => x.CreateAsync(It.IsAny<IQueryable<EmployeeForListVM>>(), 1, 10)).ReturnsAsync(paginatedVM);
            _mapper.Setup(x => x.Map<ListEmployeeForListVM>(paginatedVM)).Returns(listEmployeeForListVM);

            //Act
            var result = await _sut.GetAllPaginatedEmployeesAsync(10, 1);

            //Assert
            Assert.Equal(listEmployeeForListVM, result);
        }

        [Fact]
        public async Task UpdateEmployeeAsync_ShouldUpdateEmployeeToNewData()
        {
            //Arrange
            var employeeVM = new EmployeeVM() { Id = 12, FirstName = "unit", LastName = "test", Position = "xunit", Password = "test123", Email = "test@test.com" };
            var appUser = new ApplicationUser() { Id = "xyz123" };
            var employee = new Employee() { Id = 12, FirstName = "unit", LastName = "test", Position = "xunit", AppUser = appUser };
            var employees = new List<Employee> { employee };
            var employeesQuery = employees.AsQueryable().BuildMock();

            _employeeRepository.Setup(x => x.GetAllEmployees()).Returns(employeesQuery.Object);
            _passwordHasher.Setup(x => x.HashPassword(It.IsAny<ApplicationUser>(), It.IsAny<string>())).Returns(employee.AppUser.PasswordHash);

            //Act
            await _sut.UpdateEmployeeAsync(employeeVM);

            //Assert
            _employeeRepository.Verify(x => x.UpdateEmployeeAsync(It.IsAny<Employee>()), Times.Once);
        }

        [Fact]
        public async Task DeleteEmployeeAsync_ShouldRunsDeleteMethodOnce()
        {
            //Arrange
            var appUser = new ApplicationUser() { Id = "test123" };
            var employee = new Employee() { Id = 12, AppUser = appUser };
            var employees = new List<Employee> { employee };
            var employeesQuery = employees.AsQueryable().BuildMock();

            _employeeRepository.Setup(x => x.GetAllEmployees()).Returns(employeesQuery.Object);

            //Act
            await _sut.DeleteEmployeeAsync(employee.Id);

            //Assert
            _userManager.Verify(x => x.DeleteAsync(It.IsAny<ApplicationUser>()), Times.Once);
        }

    }
}