using System.Linq;
using System;
using System.Threading.Tasks;
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task AddOrderAsync(Order order);
        Task DeleteOrderAsync(int orderId);
        IQueryable<Order> GetAllOrders();
        Task<Order> GetOrderAsync(int orderId);
        Task UpdateOrderAsync(Order order);
    }
}
