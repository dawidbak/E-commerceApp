using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.AdminPanel;
using EcommerceApp.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EcommerceApp.Web.Tests.Controllers.UnitTests
{
    public class AdminPanelControllerUnitTests
    {
        private readonly Mock<IEmployeeService> _employeeService = new Mock<IEmployeeService>();
        private readonly Mock<ILogger<AdminPanelController>> _logger = new Mock<ILogger<AdminPanelController>>();
        private readonly AdminPanelController _sut;
        private readonly Mock<ISearchService> _searchService = new();

        public AdminPanelControllerUnitTests()
        {
            _sut = new AdminPanelController(_employeeService.Object, _logger.Object,_searchService.Object);
        }
        [Fact]
        public async Task Index_ReturnsCorrectViewResultWithAllEmployees()
        {
            //Arrange
            List<EmployeeVM> employeeVMs = new()
            {
                new EmployeeVM { FirstName = "unit", LastName = "test", Position = "xunit" },
                new EmployeeVM { FirstName = "unit2", LastName = "test2", Position = "xunit2" },
            };

            _employeeService.Setup(s => s.GetAllEmployeesAsync()).ReturnsAsync(employeeVMs);

            //Act
            var result = await _sut.Index(string.Empty,string.Empty);

            //Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<EmployeeVM>>(viewResult.Model);
            Assert.Equal(model[0].FirstName, employeeVMs[0].FirstName);
            Assert.Equal(model[1].FirstName,employeeVMs[1].FirstName);
            Assert.Equal(model.Count, employeeVMs.Count);
        }

        [Fact]
        public async Task Index_ReturnsViewResultWithSelectedEmployees()
        {
            //Arrange
            List<EmployeeVM> employeeFiltered = new()
            {
                new EmployeeVM { FirstName = "unit", LastName = "test", Position = "xunit" },
                new EmployeeVM { FirstName = "unit2", LastName = "test", Position = "xunit2" },
            };

            string selectedValue = "LastName";
            string searchString = "test";
            _searchService.Setup(x => x.SearchSelectedEmployeeAsync(selectedValue,searchString)).ReturnsAsync(employeeFiltered);

            //Act
            var result = await _sut.Index(selectedValue,searchString);

            //Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<EmployeeVM>>(viewResult.Model);
            Assert.Equal(model[0].LastName, employeeFiltered[0].LastName);
            Assert.Equal(model[1].LastName, employeeFiltered[1].LastName);
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
            _sut.ModelState.AddModelError("EmployeeName", "Required");

            //Act
            var result = await _sut.AddEmployee(employeeVM);

            //Arrange
            Assert.IsType<BadRequestResult>(result);

        }

        [Fact]
        public async Task AddEmployee_Post_ReturnsRedirectAndAddsEmployeeWhenModelStateIsValid()
        {
            //Arrange
            var employeeVM = new EmployeeVM() { FirstName = "unit", LastName = "test", Position = "xunit" };

            //Act
            var result = await _sut.AddEmployee(employeeVM);

            //Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            _employeeService.Verify(x => x.AddEmployeeAsync(It.IsAny<EmployeeVM>()), Times.Once);
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
            Assert.Equal("Index", redirectToActionResult.ActionName);
            _employeeService.Verify(x => x.DeleteEmployeeAsync(It.IsAny<int>()), Times.Once);
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
            _employeeService.Verify(x => x.GetEmployeeAsync(It.IsAny<int>()), Times.Once);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<EmployeeVM>(viewResult.ViewData.Model);
            Assert.Equal(employeeVM.Id, model.Id);
            Assert.Equal(employeeVM.FirstName, model.FirstName);
            Assert.Equal(employeeVM.LastName, model.LastName);
            Assert.Equal(employeeVM.Position, model.Position);
        }

        [Fact]
        public async Task EditEmployee_Post_ReturnsBadRequestResultWhenModelStateIsInvalid()
        {
            //Arrange
            var employeeVM = new EmployeeVM() { Id = 2, FirstName = "unit", LastName = "test", Position = "xunit" };
            _sut.ModelState.AddModelError("EmployeeName", "Required");

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
            Assert.Equal("Index", redirectToActionResult.ActionName);
            _employeeService.Verify(x => x.UpdateEmployeeAsync(It.IsAny<EmployeeVM>()), Times.Once);
        }
    }
}