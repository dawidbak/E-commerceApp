using System;
using System.Threading.Tasks;
using EcommerceApp.Application.ViewModels.EmployeePanel;
using EcommerceApp.Application.ViewModels.Order;

namespace EcommerceApp.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderCheckoutVM> GetDataForOrderCheckoutAsync(int customerId);
        Task AddOrderAsync(OrderCheckoutVM orderCheckoutVM);
        Task<ListOrderForListVM> GetAllOrdersAsync();
        Task<ListOrderForListVM> GetAllPaginatedOrdersAsync(int pageSize, int pageNumber);
        Task<OrderDetailsVM> GetOrderDetailsAsync(int orderId);
        Task DeleteOrderAsync(int orderId);
    }
}
