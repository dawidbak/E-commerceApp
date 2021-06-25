using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Domain.Interfaces
{
    public interface IOrderItemRepository
    {
        Task AddOrderItemsAsync(List<OrderItem> orderItems);
        Task DeleteOrderItemAsync(int orderItemId);
        IQueryable<OrderItem> GetAllOrderItems();
        Task<OrderItem> GetOrderItemAsync(int orderItemId);
        Task UpdateOrderItemAsync(OrderItem orderItem);
    }
}
