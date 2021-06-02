using System;
using Xunit;
using EcommerceApp.Infrastructure.Repositories;
using EcommerceApp.Domain.Models;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace EcommerceApp.Infrastructure.Repositories.UnitTests
{
    public class EmployeeRepositoryUnitTests
    {
        private readonly DbContextOptions<AppDbContext> _options;
        public EmployeeRepositoryUnitTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        }

        [Fact]
        public async Task AddEmployeAsync_CheckEmployeeAfterAdd()
        {
            //Arrange
            Employee employee = new Employee()
            {
                Id = 123,
                FirstName = "Andrew",
                LastName = "Golavsky",
                Position = "Product Management Specialist",
                Email = "test@test.com"
            };

            using (var _context = new AppDbContext(_options))
            {
                //Act
                _context.Database.EnsureCreated();
                var _sut = new EmployeeRepository(_context);
                await _sut.AddEmployeeAsync(employee);
                var employeeResult = await _context.Employees.FindAsync(employee.Id);

                //Assert
                Assert.Equal(employee, employeeResult);
            }
        }

        [Fact]
        public async Task GetEmployeeAsync_GetEmployeeAndCheckIfEqualToModel()
        {
            //Arrange
            Employee employee = new Employee()
            {
                Id = 22,
                FirstName = "unit",
                LastName = "test",
                Position = "xunit",
                Email = "test@test.com"
            };

            using (var _context = new AppDbContext(_options))
            {
                //Act
                _context.Database.EnsureCreated();
                _context.Add(employee);
                _context.SaveChanges();
                var _sut = new EmployeeRepository(_context);
                var getEmployee = await _sut.GetEmployeeAsync(employee.Id);

                //Assert
                Assert.Equal(employee.Id, getEmployee.Id);
                Assert.Equal(employee.FirstName, getEmployee.FirstName);
                Assert.Equal(employee.LastName, getEmployee.LastName);
                Assert.Equal(employee.Position, getEmployee.Position);
            }
        }

        [Fact]
        public async Task GetAllEmployeesAsync_GetListOfEmployeesAndCheckAreEqualLikeModels()
        {
            //Arrange
            Employee employee1 = new Employee() { Id = 3, FirstName = "unit", LastName = "test", Position = "xunit", Email = "test@test.com" };
            Employee employee2 = new Employee() { Id = 2, FirstName = "test", LastName = "unit", Position = "xunit xunit", Email = "test2@test.com" };
            List<Employee> employees = new() { employee1, employee2 };

            using (var _context = new AppDbContext(_options))
            {
                //Act
                await _context.AddRangeAsync(employees);
                await _context.SaveChangesAsync();
                var _sut = new EmployeeRepository(_context);
                var getEmployees = await _sut.GetAllEmployeesAsync();

                //Assert
                Assert.Equal(employees, getEmployees);
            }
        }

        [Fact]
        public async Task UpdateEmployeeAsync_ShouldUpdateEmployeeFirstNameAndLastNameAndPosition()
        {
            //Arrange
            Employee employee = new Employee() { Id = 1, FirstName = "first", LastName = "last", Position = "empty", Email = "test@test.com" };
            Employee updatedEmployee = new Employee() { Id = 1, FirstName = "Jan", LastName = "Kowalski", Position = "Junior", Email = "test2@test.com" };

            using (var _context = new AppDbContext(_options))
            {
                //Act
                _context.Database.EnsureCreated();
                await _context.AddAsync(employee);
                await _context.SaveChangesAsync();
            }
            using (var _context = new AppDbContext(_options))
            {
                //Act
                var _sut = new EmployeeRepository(_context);
                await _sut.UpdateEmployeeAsync(updatedEmployee);
                var employeeAfterUpdate = await _context.Employees.FindAsync(employee.Id);

                //Assert
                Assert.Equal(updatedEmployee.Id, employeeAfterUpdate.Id);
                Assert.Equal(updatedEmployee.FirstName, employeeAfterUpdate.FirstName);
                Assert.Equal(updatedEmployee.LastName, employeeAfterUpdate.LastName);
                Assert.Equal(updatedEmployee.Position, employeeAfterUpdate.Position);
            }
        }

        [Fact]
        public async Task DeleteEmployeeAsync_CheckEmployeeExistsAfterDelete()
        {
            //Assert
            Employee employee1 = new Employee() { Id = 98, FirstName = "unit", LastName = "test", Position = "xunit", Email = "test@test.com" };
            Employee employee2 = new Employee() { Id = 99, FirstName = "test", LastName = "unit", Position = "xunit xunit", Email = "test2@test.com" };

            using (var _context = new AppDbContext(_options))
            {
                //Act
                _context.Database.EnsureCreated();
                await _context.AddAsync(employee1);
                await _context.AddAsync(employee2);
                await _context.SaveChangesAsync();
                var _sut = new EmployeeRepository(_context);
                await _sut.DeleteEmployeeAsync(employee1.Id);
                var getEmployee1 = await _context.Employees.FindAsync(employee1.Id);
                var getEmployee2 = await _context.Employees.FindAsync(employee2.Id);

                //Assert
                Assert.Null(getEmployee1);
                Assert.Equal(employee2, getEmployee2);
            }
        }
    }
}
