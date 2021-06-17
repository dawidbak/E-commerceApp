using System;
using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Domain.Interfaces
{
    public interface IOrderItemRepository
    {
        Task AddOrderItemAsync(OrderItem orderItem);
        Task DeleteOrderItemAsync(int orderItemId);
        IQueryable<OrderItem> GetAllOrderItems();
        Task<OrderItem> GetOrderItemAsync(int orderItemId);
        Task UpdateOrderItemAsync(OrderItem orderItem);
    }
}
