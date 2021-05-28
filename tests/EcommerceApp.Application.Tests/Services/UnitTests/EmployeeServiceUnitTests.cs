using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using EcommerceApp.Application.ViewModels;
using EcommerceApp.Domain.Models;
using AutoMapper;
using EcommerceApp.Application.Mapping;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Application.Services;

namespace EcommerceApp.Application.Tests.Services.UnitTests
{
    public class EmployeeServiceUnitTests
    {
        [Fact]
        public async Task CheckEmployeeExistsAfterAdd()
        {
            //Arrange
            Employee employee = new Employee();
            EmployeeVM employeeVM = new EmployeeVM() { FirstName = "unit", LastName = "test", Position = "xunit" };

            var config = new MapperConfiguration(c => c.AddProfile(new MappingProfile()));
            var mapper = config.CreateMapper();

            var mock = new Mock<IEmployeeRepository>();

            mock.SetupAsync(m => m.AddEmployeeAsync(It.IsAny<Employee>())).Callback<Employee>(c =>
             {
                 employee = c;
             });
            var service = new EmployeeService(mapper, mock.Object);

            //Act
            await service.AddEmployeeAsync(employeeVM);

            //Assert
            mock.Verify(x => x.AddEmployeeAsync(It.IsAny<Employee>()), Times.Once);
            Assert.Equal(employee.FirstName, employeeVM.FirstName);
            Assert.Equal(employee.LastName, employeeVM.LastName);
            Assert.Equal(employee.Position, employeeVM.Position);
        }
        [Fact]
        public async Task CheckEmployeeIfEqualToModel()
        {
            //Arrange
            Employee employee = new Employee() { Id = 11, FirstName = "unit", LastName = "test", Position = "xunit" };

            var config = new MapperConfiguration(c => c.AddProfile(new MappingProfile()));
            var mapper = config.CreateMapper();

            var mock = new Mock<IEmployeeRepository>();

            mock.SetupAsync(m => m.GetEmployeeAsync(employee.Id)).Returns(employee);

            var service = new EmployeeService(mapper, mock.Object);

            //Act
            var result = await service.GetEmployeeAsync(employee.Id);

            //Assert
            Assert.Equal(employee.FirstName, result.FirstName);
            Assert.Equal(employee.LastName, result.LastName);
            Assert.Equal(employee.Position, result.Position);
        }

        [Fact]
        public async Task CheckEmployeesAreEqualsToModels()
        {
            //Arrange
            Employee employee1 = new Employee() { Id = 12, FirstName = "unit", LastName = "test", Position = "xunit" };
            Employee employee2 = new Employee() { Id = 13, FirstName = "unit1", LastName = "test1", Position = "xunit1" };
            Employee employee3 = new Employee() { Id = 14, FirstName = "unit2", LastName = "test2", Position = "xunit2" };

            List<Employee> employees = new(){employee1,employee2,employee3};

            var config = new MapperConfiguration(c => c.AddProfile(new MappingProfile()));
            var mapper = config.CreateMapper();

            var mock = new Mock<IEmployeeRepository>();

            mock.SetupAsync(m => m.GetAllEmployeesAsync()).Returns(employees.AsQueryable());

            var service = new EmployeeService(mapper, mock.Object);
            //Act
            var results = await service.GetAllEmployeesAsync();

            //Assert
            for(int i = 0; i < results.Count;i++)
            {
                Assert.Equal(employees[i].FirstName,results[i].FirstName);
                Assert.Equal(employees[i].LastName,results[i].LastName);
                Assert.Equal(employees[i].Position,results[i].Position);
            }
        }
        [Fact]
        public async Task CheckEmployeeIfCorrectUpdates()
        {
            //Arrange
            Employee employee = new Employee();
            EmployeeVM employeeVM = new EmployeeVM() { FirstName = "unit", LastName = "test", Position = "xunit" };

            var config = new MapperConfiguration(c => c.AddProfile(new MappingProfile()));
            var mapper = config.CreateMapper();

            var mock = new Mock<IEmployeeRepository>();

            mock.SetupAsync(m => m.UpdateEmployeeAsync(It.IsAny<Employee>())).Callback<Employee>(c =>
             {
                 employee = c;
             });
            var service = new EmployeeService(mapper, mock.Object);

            //Act
            await service.UpdateEmployeeAsync(employeeVM);

            //Assert
            mock.Verify(x => x.UpdateEmployeeAsync(It.IsAny<Employee>()), Times.Once);
            Assert.Equal(employee.FirstName, employeeVM.FirstName);
            Assert.Equal(employee.LastName, employeeVM.LastName);
            Assert.Equal(employee.Position, employeeVM.Position);
        }

        [Fact]
        public async Task CheckIfEmployeeDeleteMethodPassesToRepository()
        {
            //Arrange
            Employee employee1 = new Employee();
            Employee employee2 = new Employee() {Id = 1, FirstName = "unit", LastName = "test", Position = "xunit" };

            var config = new MapperConfiguration(c => c.AddProfile(new MappingProfile()));
            var mapper = config.CreateMapper();

            var mock = new Mock<IEmployeeRepository>();

            mock.Setup(m => m.DeleteEmployeeAsync(It.IsAny<int>())).Callback<int>(c => 
            {
                employee1.Id = c;
            });
            var service = new EmployeeService(mapper, mock.Object);

            //Act
            await service.DeleteEmployeeAsync(employee2.Id);

            //Assert
            mock.Verify(x => x.DeleteEmployeeAsync(It.IsAny<int>()), Times.Once);
            Assert.Equal(employee1.Id,employee2.Id);
        }
    }
}