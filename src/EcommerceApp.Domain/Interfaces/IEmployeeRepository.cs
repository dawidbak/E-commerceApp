using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Domain.Interfaces
{
    public interface IEmployeeRepository
    {
        Task AddEmployeeAsync(Employee emploee);
        Task<Employee> GetEmployeeAsync(int employeeID);
        Task<IQueryable<Employee>> GetAllEmployeesAsync();
        Task UpdateEmployeeAsync(int employeeID);
        Task DeleteEmployeeAsync(int employeeID);
    }
}