using System;
using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Infrastructure.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly AppDbContext _appDbContext;

        public OrderItemRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task AddOrderItemAsync(OrderItem orderItem)
        {
            await _appDbContext.OrderItems.AddAsync(orderItem);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteOrderItemAsync(int orderItemId)
        {
            var orderItem = await _appDbContext.OrderItems.FindAsync(orderItemId);
            if (orderItem != null)
            {
                _appDbContext.OrderItems.Remove(orderItem);
                await _appDbContext.SaveChangesAsync();
            }
        }

        public async Task<IQueryable<OrderItem>> GetAllOrderItemsAsync()
        {
            return (await _appDbContext.OrderItems.ToListAsync()).AsQueryable();
        }

        public async Task<OrderItem> GetOrderItemAsync(int orderItemId)
        {
            return await _appDbContext.OrderItems.FindAsync(orderItemId);
        }

        public async Task UpdateOrderItemAsync(OrderItem orderItem)
        {
            _appDbContext.OrderItems.Update(orderItem);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
