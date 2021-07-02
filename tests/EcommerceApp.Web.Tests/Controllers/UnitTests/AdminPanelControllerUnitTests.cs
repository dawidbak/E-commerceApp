using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.AdminPanel;
using EcommerceApp.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EcommerceApp.Web.Tests.Controllers.UnitTests
{
    public class AdminPanelControllerUnitTests
    {
        private readonly Mock<IEmployeeService> _employeeService = new();
        private readonly Mock<ICustomerService> _customerService = new();
        private readonly Mock<ILogger<AdminPanelController>> _logger = new();
        private readonly AdminPanelController _sut;
        private readonly Mock<ISearchService> _searchService = new();
        private readonly Mock<IConfiguration> _configuration = new();

        public AdminPanelControllerUnitTests()
        {
            _sut = new AdminPanelController
            (
                _employeeService.Object,
                _logger.Object,
                _searchService.Object,
                _customerService.Object,
                _configuration.Object
            );
        }

        [Fact]
        public async Task Employees_ReturnsViewResultWithAllEmployees()
        {
            //Arrange
            var employeeForListVMs = new List<EmployeeForListVM>
            {
                new EmployeeForListVM() { FirstName = "unit", LastName = "test", Position = "xunit" },
                new EmployeeForListVM() { FirstName = "unit1", LastName = "test1", Position = "xunit1" },
            };
            var listEmployeeForListVM = new ListEmployeeForListVM
            {
                Employees = employeeForListVMs,
                TotalPages = 1,
                CurrentPage = 1
            };

            _employeeService.Setup(x => x.GetAllPaginatedEmployeesAsync(10, 1)).ReturnsAsync(listEmployeeForListVM);

            //Act
            var result = await _sut.Employees(string.Empty, string.Empty, "10", 1);

            //Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ListEmployeeForListVM>(viewResult.Model);
            Assert.Equal(listEmployeeForListVM.Employees, model.Employees);
            Assert.Equal(listEmployeeForListVM.TotalPages, model.TotalPages);
            Assert.Equal(listEmployeeForListVM.CurrentPage, model.CurrentPage);
        }

        [Fact]
        public async Task Employees_ReturnsViewResultWithSearchedEmployees()
        {
            //Arrange
            var employeeForListVMs = new List<EmployeeForListVM>
            {
                new EmployeeForListVM() { FirstName = "unit1", LastName = "test1", Position = "xunit1" },
            };
            var listEmployeeForListVM = new ListEmployeeForListVM
            {
                Employees = employeeForListVMs,
                TotalPages = 1,
                CurrentPage = 1
            };

            string selectedValue = "LastName";
            string searchString = "test1";
            _searchService.Setup(x => x.SearchSelectedEmployeesAsync(selectedValue, searchString, 10, 1)).ReturnsAsync(listEmployeeForListVM);

            //Act
            var result = await _sut.Employees(selectedValue, searchString, "10", 1);

            //Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ListEmployeeForListVM>(viewResult.Model);
            Assert.Equal(listEmployeeForListVM.Employees, model.Employees);
        }

        [Fact]
        public async Task Customers_ReturnsViewResultWithAllCustomers()
        {
            //Arrange
            var customerVMs = new List<CustomerVM>
            {
                new CustomerVM() { FirstName = "unit", LastName = "test"},
                new CustomerVM() { FirstName = "unit1", LastName = "test1"},
            };
            var listCustomerVM = new ListCustomerVM
            {
                Customers = customerVMs,
                TotalPages = 1,
                CurrentPage = 1
            };

            _customerService.Setup(x => x.GetAllPaginatedCustomersAsync(10, 1)).ReturnsAsync(listCustomerVM);

            //Act
            var result = await _sut.Customers(string.Empty, string.Empty, "10", 1);

            //Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ListCustomerVM>(viewResult.Model);
            Assert.Equal(listCustomerVM.Customers, model.Customers);
            Assert.Equal(listCustomerVM.TotalPages, model.TotalPages);
            Assert.Equal(listCustomerVM.CurrentPage, model.CurrentPage);
        }

        [Fact]
        public async Task Customers_ReturnsViewResultWithSearchedCustomers()
        {
            //Arrange
            var customerVMs = new List<CustomerVM>
            {
                new CustomerVM() { FirstName = "unit1", LastName = "test1"},
            };
            var listCustomerVM = new ListCustomerVM
            {
                Customers = customerVMs,
                TotalPages = 1,
                CurrentPage = 1
            };
            string selectedValue = "LastName";
            string searchString = "test1";
            _searchService.Setup(x => x.SearchSelectedCustomersAsync(selectedValue, searchString, 10, 1)).ReturnsAsync(listCustomerVM);

            //Act
            var result = await _sut.Customers(selectedValue, searchString, "10", 1);

            //Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ListCustomerVM>(viewResult.Model);
            Assert.Equal(listCustomerVM.Customers, model.Customers);
            Assert.Equal(listCustomerVM.TotalPages, model.TotalPages);
            Assert.Equal(listCustomerVM.CurrentPage, model.CurrentPage);
        }

        [Fact]
        public void AddEmployee_Get_ReturnCorrectViewResult()
        {
            //Arrange

            //Act
            var result = _sut.AddEmployee();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task AddEmployee_Post_ReturnsBadRequestResultWhenModelStateIsInvalid()
        {
            //Arrange
            var employeeVM = new EmployeeVM() { Id = 1, FirstName = "n", LastName = "test", Position = "xunit" };
            _sut.ModelState.AddModelError("EmployeeVM", "Invalid");

            //Act
            var result = await _sut.AddEmployee(employeeVM);

            //Arrange
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task AddEmployee_Post_ReturnsRedirectAndAddsEmployeeWhenModelStateIsValid()
        {
            //Arrange
            var employeeVM = new EmployeeVM()
            {
                Id = 1,
                FirstName = "unit",
                LastName = "test",
                Position = "xunit",
                Password = "Pa$$w0rd!",
                Email = "test@test.com"
            };

            //Act
            var result = await _sut.AddEmployee(employeeVM);

            //Assert
            _employeeService.Verify(x => x.AddEmployeeAsync(employeeVM), Times.Once);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Employees", redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task DeleteEmployee_ReturnsNotFoundWhenIdIsNull()
        {
            //Arrange
            int? id = null;

            //Act
            var result = await _sut.DeleteEmployee(id);

            //Assert
            var redirectToNotFoundPage = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, redirectToNotFoundPage.StatusCode);
        }

        [Fact]
        public async Task DeleteEmployee_DeleteEmployeeAndRedirectToActionWhenIdHasValue()
        {
            //Arrange
            int id = 1;

            //Act
            var result = await _sut.DeleteEmployee(id);

            //Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Employees", redirectToActionResult.ActionName);
            _employeeService.Verify(x => x.DeleteEmployeeAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task DeleteCustomer_ReturnsNotFoundWhenIdIsNull()
        {
            //Arrange
            int? id = null;

            //Act
            var result = await _sut.DeleteEmployee(id);

            //Assert
            var redirectToNotFoundPage = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, redirectToNotFoundPage.StatusCode);
        }

        [Fact]
        public async Task DeleteCustomer_DeleteCustomerAndRedirectToActionWhenIdHasValue()
        {
            //Arrange
            int id = 1;

            //Act
            var result = await _sut.DeleteCustomer(id);

            //Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Customers", redirectToActionResult.ActionName);
            _customerService.Verify(x => x.DeleteCustomerAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task EditEmployee_Get_ReturnsNotFoundWhenIdIsNull()
        {
            //Arrange
            int? id = null;

            //Act
            var result = await _sut.EditEmployee(id);

            //Assert
            var redirectToNotFoundPage = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, redirectToNotFoundPage.StatusCode);
        }

        [Fact]
        public async Task EditEmployee_Get_ReturnsViewResultWithEmployeeViewModel()
        {
            //Arrange
            var employeeVM = new EmployeeVM() { Id = 2, FirstName = "unit", LastName = "test", Position = "xunit" };
            _employeeService.Setup(x => x.GetEmployeeAsync(employeeVM.Id)).ReturnsAsync(employeeVM);

            //Act
            var result = await _sut.EditEmployee(employeeVM.Id);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<EmployeeVM>(viewResult.ViewData.Model);
            Assert.Equal(employeeVM, model);
        }

        [Fact]
        public async Task EditEmployee_Post_ReturnsBadRequestResultWhenModelStateIsInvalid()
        {
            //Arrange
            var employeeVM = new EmployeeVM() { Id = 2, FirstName = "unit", LastName = "test", Position = "xunit" };
            _sut.ModelState.AddModelError("EmployeeVM", "Invalid");

            //Act
            var result = await _sut.EditEmployee(employeeVM);

            //Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task EditEmployee_Post_ReturnsARedirectAndUpdatesEmployeeWhenModelStateIsValid()
        {
            //Arrange
            var employeeVM = new EmployeeVM() { Id = 2, FirstName = "unit", LastName = "test", Position = "xunit" };

            //Act
            var result = await _sut.EditEmployee(employeeVM);

            //Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Employees", redirectToActionResult.ActionName);
            _employeeService.Verify(x => x.UpdateEmployeeAsync(It.IsAny<EmployeeVM>()), Times.Once);
        }

        [Fact]
        public async Task CustomerDetails_ReturnsNotFoundWhenIdIsNull()
        {
            //Arrange
            int? id = null;

            //Act
            var result = await _sut.CustomerDetails(id);

            //Assert
            var redirectToNotFoundPage = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, redirectToNotFoundPage.StatusCode);
        }

        [Fact]
        public async Task CustomerDetails_ReturnsViewResultWithCustomerDetailsVM()
        {
            //Arrange
            var customerDetailsVM = new CustomerDetailsVM() { Id = 2, FirstName = "unit", LastName = "test" };

            _customerService.Setup(x => x.GetCustomerDetailsAsync(customerDetailsVM.Id)).ReturnsAsync(customerDetailsVM);
            //Act
            var result = await _sut.CustomerDetails(customerDetailsVM.Id);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CustomerDetailsVM>(viewResult.ViewData.Model);
            Assert.Equal(customerDetailsVM, model);
        }
    }
}