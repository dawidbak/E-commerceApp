using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _appDbContext;

        public EmployeeRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task AddEmployeeAsync(Employee emploee)
        {
            await _appDbContext.AddAsync(emploee);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteEmployeeAsync(int employeeID)
        {
            var employee = await _appDbContext.Employees.FindAsync(employeeID);
            if(employee != null)
            {
                _appDbContext.Remove(employee);
                await _appDbContext.SaveChangesAsync();
            }
        }

        public async Task<IQueryable<Employee>> GetAllEmployeesAsync()
        {
            return (await _appDbContext.Employees.ToListAsync()).AsQueryable();
        }

        public async Task<Employee> GetEmployeeAsync(int employeeID)
        {
            return await _appDbContext.Employees.FindAsync(employeeID);
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            var employeeToUpdate = await _appDbContext.Employees.FindAsync(employee.ID);
            _appDbContext.Update(employeeToUpdate);
            await _appDbContext.SaveChangesAsync();
        }
    }
}