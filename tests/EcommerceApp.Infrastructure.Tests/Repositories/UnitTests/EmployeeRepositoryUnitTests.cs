using System;
using Xunit;
using EcommerceApp.Infrastructure.Repositories;
using EcommerceApp.Domain.Models;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace EcommerceApp.Infrastructure.Repositories.UnitTests
{
    public class EmployeeRepositoryUnitTests
    {
        [Fact]
        public async Task CheckEmployeeAfterAdd()
        {
            //Arrange
            Employee employee = new Employee()
            {
                Id = 123,
                FirstName = "Andrew",
                LastName = "Golavsky",
                Position = "Product Management Specialist"
            };
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connection).Options;


            using (var context = new AppDbContext(options))
            {
                //Act
                context.Database.EnsureCreated();
                var employeeRepository = new EmployeeRepository(context);
                await employeeRepository.AddEmployeeAsync(employee);
                var employeeResult = await context.Employees.FindAsync(employee.Id);

                //Assert
                Assert.Equal(employee, employeeResult);
            }
        }

        [Fact]
        public async Task GetEmployeeAndCheckIfEqualToModel()
        {
            //Arrange
            Employee employee = new Employee()
            {
                Id = 22,
                FirstName = "unit",
                LastName = "test",
                Position = "xunit"
            };
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connection).Options;

            using (var context = new AppDbContext(options))
            {
                //Act
                context.Database.EnsureCreated();
                context.Add(employee);
                context.SaveChanges();
                var employeeRepository = new EmployeeRepository(context);
                var getEmployee = await employeeRepository.GetEmployeeAsync(employee.Id);

                //Assert
                Assert.Equal(employee, getEmployee);
            }
        }

        [Fact]
        public async Task GetListOfEmployeesAndCheckAreEqualLikeModels()
        {
            //Arrange
            Employee employee1 = new Employee(){Id = 1, FirstName = "unit", LastName = "test",Position="xunit"};
            Employee employee2 = new Employee(){Id = 2, FirstName = "test", LastName ="unit", Position = "xunit xunit"};

            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connection).Options;

            using (var context = new AppDbContext(options))
            {
                //Act
                context.Database.EnsureCreated();
                await context.AddAsync(employee1);
                await context.AddAsync(employee2);
                await context.SaveChangesAsync();
                var employeeRepository = new EmployeeRepository(context);
                var getEmployees = await employeeRepository.GetAllEmployeesAsync();

                //Assert
                Assert.Equal(employee1, getEmployees.FirstOrDefault(x=>x.Id == employee1.Id));
                Assert.Equal(employee2, getEmployees.FirstOrDefault(x=>x.Id == employee2.Id));
            }
        }

        [Fact]
        public async Task ShouldUpdateEmployeeFirstNameAndLastNameAndPosition()
        {
            //Arrange
            Employee employee = new Employee(){Id = 1, FirstName = "first", LastName = "last",Position="empty"};
            Employee updatedEmployee = new Employee(){Id = 1, FirstName = "Jan", LastName ="Kowalski", Position = "Junior"};

            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connection).Options;

            using(var context = new AppDbContext(options))
            {
                //Act
                context.Database.EnsureCreated();
                await context.AddAsync(employee);
                await context.SaveChangesAsync();
                
            }
            using(var context = new AppDbContext(options))
            {
                //Act
                var employeeRepository = new EmployeeRepository(context);
                await employeeRepository.UpdateEmployeeAsync(updatedEmployee);
                var employeeAfterUpdate = await context.Employees.FindAsync(employee.Id);

                //Assert
                Assert.Equal(updatedEmployee.Id,employeeAfterUpdate.Id);
                Assert.Equal(updatedEmployee.FirstName,employeeAfterUpdate.FirstName);
                Assert.Equal(updatedEmployee.LastName,employeeAfterUpdate.LastName);
                Assert.Equal(updatedEmployee.Position,employeeAfterUpdate.Position);
            }
            
        }

        [Fact]
        public async Task CheckEmployeeExistsAfterDelete()
        {
            //Assert
            Employee employee1 = new Employee(){Id = 98, FirstName = "unit", LastName = "test",Position="xunit"};
            Employee employee2 = new Employee(){Id = 99, FirstName = "test", LastName ="unit", Position = "xunit xunit"};

            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connection).Options;

            using (var context = new AppDbContext(options))
            {
                //Act
                context.Database.EnsureCreated();
                var employeeRepository = new EmployeeRepository(context);
                await context.AddAsync(employee1);
                await context.AddAsync(employee2);
                await context.SaveChangesAsync();
                await employeeRepository.DeleteEmployeeAsync(employee1.Id);
                var getEmployee1 = await context.Employees.FindAsync(employee1.Id);
                var getEmployee2 = await context.Employees.FindAsync(employee2.Id);

                //Assert
                Assert.Null(getEmployee1);
                Assert.Equal(employee2, getEmployee2);
            }
        }
    }
}
