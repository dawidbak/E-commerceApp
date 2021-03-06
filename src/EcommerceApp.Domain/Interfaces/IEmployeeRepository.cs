using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Domain.Interfaces
{
    public interface IEmployeeRepository
    {
        Task AddEmployeeAsync(Employee emploee);
        Task<Employee> GetEmployeeAsync(int employeeID);
        IQueryable<Employee> GetAllEmployees();
        Task UpdateEmployeeAsync(Employee employee);
        Task DeleteEmployeeAsync(int employeeID);
    }
}