using System;
using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Domain.Interfaces
{
    public interface ICustomerRepository
    {
        Task AddCustomerAsync(Customer customer);
        Task<IQueryable<Customer>> GetAllCustomersAsync();
        Task<Customer> GetCustomerAsync(int customerId);
        Task<int> GetCustomerIdAsync(string AppUserId);
        Task UpdateCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(int customerId);
    }
}