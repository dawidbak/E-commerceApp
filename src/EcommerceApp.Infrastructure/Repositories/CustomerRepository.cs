using System;
using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _appDbContext;

        public CustomerRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            await _appDbContext.AddAsync(customer);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<IQueryable<Customer>> GetAllCustomersAsync()
        {
            return (await _appDbContext.Customers.ToListAsync()).AsQueryable();
        }

        public async Task<Customer> GetCustomerAsync(int customerId)
        {
            return await _appDbContext.Customers.FindAsync(customerId);
        }

        public async Task<int> GetCustomerIdAsync(string AppUserId)
        {
            var customer = await _appDbContext.Customers.AsNoTracking().FirstOrDefaultAsync(x => x.AppUserId == AppUserId);
            return customer.Id;
        }   
        public async Task UpdateCustomerAsync(Customer customer)
        {
            _appDbContext.Update(customer);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteCustomerAsync(int customerId)
        {
            var customer = await _appDbContext.Customers.FindAsync(customerId);
            if (customer != null)
            {
                _appDbContext.Remove(customer);
                await _appDbContext.SaveChangesAsync();
            }
        }
    }
}
