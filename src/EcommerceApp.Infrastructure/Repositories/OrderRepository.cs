using System;
using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _appDbContext;

        public OrderRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task AddOrderAsync(Order order)
        {
            await _appDbContext.AddAsync(order);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int orderId)
        {
            var order = await _appDbContext.Orders.FindAsync(orderId);
            if (order != null)
            {
                _appDbContext.Orders.Remove(order);
                await _appDbContext.SaveChangesAsync();
            }
        }

        public IQueryable<Order> GetAllOrders()
        {
            return _appDbContext.Orders.AsQueryable();
        }

        public async Task<Order> GetOrderAsync(int orderId)
        {
            return await _appDbContext.Orders.FindAsync(orderId);
        }

        public async Task UpdateOrderAsync(Order order)
        {
            _appDbContext.Orders.Update(order);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
