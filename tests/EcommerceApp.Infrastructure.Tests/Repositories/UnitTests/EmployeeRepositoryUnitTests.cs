using System;
using Xunit;
using EcommerceApp.Infrastructure.Repositories;
using EcommerceApp.Domain.Models;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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
                ID = 123,
                FirstName = "Andrew",
                LastName = "Golavsky",
                Position = "Product Management Specialist"
            };
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connection).Options;
            
            
            using(var context = new AppDbContext(options))
            {
                //Act
                context.Database.EnsureCreated();
                var employeeRepository = new EmployeeRepository(context);
                await employeeRepository.AddEmployeeAsync(employee);
                var employeeResult = await context.Employees.FindAsync(employee.ID);

                //Assert
                Assert.Equal(employee,employeeResult);
            }
        }
    }
}
