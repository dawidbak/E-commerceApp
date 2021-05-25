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

        public async Task DeleteEmployeeAsync(int employeeId)
        {
            var employee = await _appDbContext.Employees.FindAsync(employeeId);
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

        public async Task<Employee> GetEmployeeAsync(int employeeId)
        {
            return await _appDbContext.Employees.FindAsync(employeeId);
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            _appDbContext.Update(employee);
            await _appDbContext.SaveChangesAsync();
        }
    }
}