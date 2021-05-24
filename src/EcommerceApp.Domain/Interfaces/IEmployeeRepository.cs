using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Domain.Interfaces
{
    public interface IEmployeeRepository
    {
        Task AddEmployee(Employee emploee);
        Task<Employee> GetEmployee(int employeeID);
        Task<IQueryable<Employee>> GetAllEmployees();
        Task UpdateEmployee(int employeeID);
        Task DeleteEmployee(int employeeID);
    }
}