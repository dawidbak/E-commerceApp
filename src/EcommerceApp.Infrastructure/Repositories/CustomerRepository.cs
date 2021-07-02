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

        public IQueryable<Customer> GetAllCustomers()
        {
            return _appDbContext.Customers.AsQueryable();
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
    }
}
