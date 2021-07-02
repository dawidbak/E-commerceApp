using System;
using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Domain.Interfaces
{
    public interface ICustomerRepository
    {
        IQueryable<Customer> GetAllCustomers();
        Task<int> GetCustomerIdAsync(string AppUserId);
        Task UpdateCustomerAsync(Customer customer);
    }
}
