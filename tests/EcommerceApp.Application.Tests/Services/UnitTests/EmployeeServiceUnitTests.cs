using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using EcommerceApp.Application.ViewModels.AdminPanel;
using EcommerceApp.Domain.Models;
using AutoMapper;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Application.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace EcommerceApp.Application.Tests.Services.UnitTests
{
    public class EmployeeServiceUnitTests
    {
        private readonly EmployeeService _sut;
        private readonly Mock<IMapper> _mapper = new();
        private readonly Mock<IEmployeeRepository> _employeeRepository = new();
        private readonly Mock<UserManager<ApplicationUser>> _userManager;

        public EmployeeServiceUnitTests()
        {
            var mockUserStore = new Mock<IUserStore<ApplicationUser>>();
            _userManager = new Mock<UserManager<ApplicationUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);
            _sut = new EmployeeService(_mapper.Object,_employeeRepository.Object,_userManager.Object);
        }

        [Fact]
        public async Task AddEmployee_ShouldEmployeeExistsAfterAdd()
        {
            //Arrange
            var employee = new Employee(){ FirstName = "unit", LastName = "test", Position = "xunit",Email="test@test.com"};
            var employeeVM = new EmployeeVM(){ FirstName = "unit", LastName = "test", Position = "xunit",Email="test@test.com"};

            _mapper.Setup(x => x.Map<Employee>(employeeVM)).Returns(employee);
            //Act
            await _sut.AddEmployeeAsync(employeeVM);

            //Assert
            _employeeRepository.Verify(x => x.AddEmployeeAsync(It.IsAny<Employee>()), Times.Once);
            _userManager.Verify(x => x.CreateAsync(It.IsAny<ApplicationUser>(),It.IsAny<string>()), Times.Once);
            _userManager.Verify(x => x.AddClaimAsync(It.IsAny<ApplicationUser>(), It.IsAny<Claim>()), Times.Once);
            Assert.Equal(employee.FirstName, employeeVM.FirstName);
            Assert.Equal(employee.LastName, employeeVM.LastName);
            Assert.Equal(employee.Position, employeeVM.Position);
            Assert.Equal(employee.Email, employeeVM.Email);
        }
        
        [Fact]
        public async Task GetEmployeeAsync_ShouldReturnEmployeeVM()
        {
            //Arrange
            var employee = new Employee() { Id = 11, FirstName = "unit", LastName = "test", Position = "xunit",Email="test@test.com" };
            var employeeVM = new EmployeeVM() { FirstName = "unit", LastName = "test", Position = "xunit",Email="test@test.com"};

            _employeeRepository.Setup(x => x.GetEmployeeAsync(employee.Id)).ReturnsAsync(employee);
            _mapper.Setup(x => x.Map<EmployeeVM>(employee)).Returns(employeeVM);

            //Act
            var result = await _sut.GetEmployeeAsync(employee.Id);

            //Assert
            Assert.Equal(employee.FirstName, result.FirstName);
            Assert.Equal(employee.LastName, result.LastName);
            Assert.Equal(employee.Position, result.Position);
            Assert.Equal(employee.Email, result.Email);
        }

        [Fact]
        public async Task GetAllEmployeesAsync_ShouldReturnsListOfEmployees()
        {
            //Arrange
            var employee1 = new Employee() { Id = 12, FirstName = "unit", LastName = "test", Position = "xunit" };
            var employee2 = new Employee() { Id = 13, FirstName = "unit1", LastName = "test1", Position = "xunit1" };
            List<Employee> employees = new() { employee1, employee2};
            var employeeVM1 = new EmployeeVM() {FirstName = "unit", LastName = "test", Position = "xunit" };
            var employeeVM2 = new EmployeeVM() {FirstName = "unit1", LastName = "test1", Position = "xunit1" };
            List<EmployeeVM> employeesVM = new() { employeeVM1, employeeVM2};

            _employeeRepository.Setup(x => x.GetAllEmployeesAsync()).ReturnsAsync(employees.AsQueryable);
            _mapper.Setup(x => x.Map<List<EmployeeVM>>(employees)).Returns(employeesVM);

            //Act
            var results = await _sut.GetAllEmployeesAsync();

            //Assert
            for (int i = 0; i < results.Count; i++)
            {
                Assert.Equal(employees[i].FirstName, results[i].FirstName);
                Assert.Equal(employees[i].LastName, results[i].LastName);
                Assert.Equal(employees[i].Position, results[i].Position);
            }
        }
        
        [Fact]
        public async Task UpdateEmployeeAsync_ShouldUpdateEmployee()
        {
            //Arrange
            var employeeVM = new EmployeeVM() { Id = 12,FirstName = "unit", LastName = "test", Position = "xunit", Password ="test123"};
            var employee = new Employee() { Id = 12,FirstName = "unit", LastName = "test", Position = "xunit", AppUserId ="xyz123"};
            var appUser = new ApplicationUser(){Id ="xyz123"};

            _employeeRepository.Setup(x => x.GetEmployeeAsync(employeeVM.Id)).ReturnsAsync(employee);
            _userManager.Setup(x => x.FindByIdAsync(employee.AppUserId)).ReturnsAsync(appUser);
            _mapper.Setup(x => x.Map<Employee>(employeeVM)).Returns(employee);

            //Act
            await _sut.UpdateEmployeeAsync(employeeVM);

            //Assert
            _userManager.Verify(x => x.SetEmailAsync(It.IsAny<ApplicationUser>(),It.IsAny<string>()), Times.Once);
            _userManager.Verify(x => x.UpdateNormalizedEmailAsync(It.IsAny<ApplicationUser>()), Times.Once);
            _userManager.Verify(x => x.UpdateNormalizedUserNameAsync(It.IsAny<ApplicationUser>()), Times.Once);
            _userManager.Verify(x => x.SetUserNameAsync(It.IsAny<ApplicationUser>(),It.IsAny<string>()), Times.Once);
            _userManager.Verify(x => x.ResetPasswordAsync(It.IsAny<ApplicationUser>(),It.IsAny<string>(),It.IsAny<string>()), Times.Once);
            _userManager.Verify(x => x.ResetPasswordAsync(It.IsAny<ApplicationUser>(),It.IsAny<string>(),null), Times.Never);
            Assert.Equal(employee.Id, employeeVM.Id);
            Assert.Equal(appUser.Id, employee.AppUserId);
        }

        [Fact]
        public async Task DeleteEmployeeAsync_ShouldFetchEmployeeAndUserFromDbAndPassDeleteMethods()
        {
            //Arrange
            var employeeVM = new EmployeeVM() {Id = 12};
            var employee = new Employee() {Id = 12, AppUserId ="test123"};
            var appUser = new ApplicationUser(){Id = "test123"};

            _employeeRepository.Setup(x => x.GetEmployeeAsync(employeeVM.Id)).ReturnsAsync(employee);
            _userManager.Setup(x => x.FindByIdAsync(employee.AppUserId)).ReturnsAsync(appUser);

            //Act
            await _sut.DeleteEmployeeAsync(employeeVM.Id);

            //Assert
            Assert.Equal(appUser.Id, employee.AppUserId);
            _userManager.Verify(x => x.DeleteAsync(It.IsAny<ApplicationUser>()), Times.Once);
            _employeeRepository.Verify(x => x.DeleteEmployeeAsync(It.IsAny<int>()), Times.Once);
        }
        
    }
}