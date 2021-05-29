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
using Microsoft.AspNetCore.Identity;

namespace EcommerceApp.Application.Tests.Services.UnitTests
{
    public class EmployeeServiceUnitTests
    {
        public Mock<UserManager<ApplicationUser>> GetMockUserManager()
        {
            var mockUserStore = new Mock<IUserStore<ApplicationUser>>();
            return new Mock<UserManager<ApplicationUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

        }

        [Fact]
        public async Task CheckEmployeeExistsAfterAdd()
        {
            //Arrange
            Employee employee = new Employee();
            EmployeeVM employeeVM = new EmployeeVM() { FirstName = "unit", LastName = "test", Position = "xunit",Email="test@test.com", Password = "Pa$$w0rd!"};
            EmployeeVM employeeVM2 = new EmployeeVM();
            var appUser = new ApplicationUser(){UserName= employeeVM.Email, Email = employeeVM.Email};
            var config = new MapperConfiguration(c => c.AddProfile(new MappingProfile()));
            var mapper = config.CreateMapper();
            var userManagerMock = GetMockUserManager();
            

            var mock = new Mock<IEmployeeRepository>();

            userManagerMock.Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>(),It.IsAny<string>())).Callback<ApplicationUser,string>((a,c) => 
            {
                employeeVM2.Email = a.Email;
                employeeVM2.Password = c;
            });
            mock.SetupAsync(m => m.AddEmployeeAsync(It.IsAny<Employee>())).Callback<Employee>(c =>
             {
                 employee = c;
             });
            var service = new EmployeeService(mapper, mock.Object, userManagerMock.Object);

            //Act
            await service.AddEmployeeAsync(employeeVM);

            //Assert
            mock.Verify(x => x.AddEmployeeAsync(It.IsAny<Employee>()), Times.Once);
            userManagerMock.Verify(x => x.CreateAsync(It.IsAny<ApplicationUser>(),It.IsAny<string>()), Times.Once);
            Assert.Equal(employee.FirstName, employeeVM.FirstName);
            Assert.Equal(employee.LastName, employeeVM.LastName);
            Assert.Equal(employee.Position, employeeVM.Position);
            Assert.Equal(employeeVM2.Email, employeeVM.Email);
            Assert.Equal(employeeVM2.Password,employeeVM.Password);
        }
        [Fact]
        public async Task CheckEmployeeIfEqualToModel()
        {
            //Arrange
            Employee employee = new Employee() { Id = 11, FirstName = "unit", LastName = "test", Position = "xunit" };

            var config = new MapperConfiguration(c => c.AddProfile(new MappingProfile()));
            var mapper = config.CreateMapper();

            var userManagerMock = GetMockUserManager();

            var mock = new Mock<IEmployeeRepository>();

            mock.SetupAsync(m => m.GetEmployeeAsync(employee.Id)).Returns(employee);

            var service = new EmployeeService(mapper, mock.Object, userManagerMock.Object);

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

            List<Employee> employees = new() { employee1, employee2, employee3 };

            var config = new MapperConfiguration(c => c.AddProfile(new MappingProfile()));
            var mapper = config.CreateMapper();

            var userManagerMock = GetMockUserManager();  
            
            var mock = new Mock<IEmployeeRepository>();

            mock.SetupAsync(m => m.GetAllEmployeesAsync()).Returns(employees.AsQueryable());

            var service = new EmployeeService(mapper, mock.Object, userManagerMock.Object);
            //Act
            var results = await service.GetAllEmployeesAsync();

            //Assert
            for (int i = 0; i < results.Count; i++)
            {
                Assert.Equal(employees[i].FirstName, results[i].FirstName);
                Assert.Equal(employees[i].LastName, results[i].LastName);
                Assert.Equal(employees[i].Position, results[i].Position);
            }
        }
        [Fact]
        public async Task CheckEmployeeIfCorrectUpdates()
        {
            //Arrange
            var employee = new Employee();
            var employeeVM = new EmployeeVM() {FirstName = "unit", LastName = "test", Position = "xunit",Email="test@test.com", Password = "Pa$$w0rd!"};
            var employeeVM2 = new EmployeeVM();
            var appUser = new ApplicationUser(){Id ="asdasd23asdas",UserName = "test@test.com", Email = "test@test.com"};
            var appUserEmpty = new ApplicationUser();

            var config = new MapperConfiguration(c => c.AddProfile(new MappingProfile()));
            var mapper = config.CreateMapper();

            var userManagerMock = GetMockUserManager();

            var mock = new Mock<IEmployeeRepository>();

            userManagerMock.SetupAsync(m => m.FindByIdAsync(It.IsAny<string>())).Returns(appUser);

            userManagerMock.Setup(m => m.SetEmailAsync(appUser,It.IsAny<string>())).Callback<ApplicationUser,string>((a,s)=>
            {
                appUserEmpty.Id = appUser.Id;
                appUserEmpty.UserName = appUser.UserName;
                appUserEmpty.Email = appUser.Email;
                Assert.Equal(appUserEmpty.Id, appUser.Id);
                Assert.Equal(appUserEmpty.UserName, appUser.UserName);
                Assert.Equal(appUserEmpty.Email, appUser.Email);
            });

            mock.SetupAsync(m => m.UpdateEmployeeAsync(It.IsAny<Employee>())).Callback<Employee>(c =>
             {
                 employee = c;
             });

            
            var service = new EmployeeService(mapper, mock.Object, userManagerMock.Object);

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
            Employee employee2 = new Employee() { Id = 1, FirstName = "unit", LastName = "test", Position = "xunit" };
            ApplicationUser appUser = new ApplicationUser() { UserName = "db@db.com", Id = "1" };
            var config = new MapperConfiguration(c => c.AddProfile(new MappingProfile()));
            var mapper = config.CreateMapper();
            var userManagerMock = GetMockUserManager();

            var mock = new Mock<IEmployeeRepository>();

            mock.Setup(m => m.DeleteEmployeeAsync(It.IsAny<int>())).Callback<int>(c =>
            {
                employee1.Id = c;

            });

            var service = new EmployeeService(mapper, mock.Object, userManagerMock.Object);

            //Act
            await service.DeleteEmployeeAsync(employee2.Id);

            //Assert
            mock.Verify(x => x.DeleteEmployeeAsync(It.IsAny<int>()), Times.Once);
            Assert.Equal(employee1.Id, employee2.Id);
        }
    }
}