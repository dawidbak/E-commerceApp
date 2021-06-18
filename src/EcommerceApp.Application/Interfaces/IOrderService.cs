using System;
using System.Threading.Tasks;
using EcommerceApp.Application.ViewModels.EmployeePanel;
using EcommerceApp.Application.ViewModels.Order;

namespace EcommerceApp.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderCheckoutVM> GetDataForOrderCheckoutAsync(int cartId);
        Task AddOrderAsync(OrderCheckoutVM orderCheckoutVM);
        Task<ListOrderForListVM> GetAllOrdersAsync();
        Task<ListOrderForListVM> GetAllPaginatedOrdersAsync(int pageSize, int pageNumber);
        Task<OrderDetailsVM> GetOrderDetailsAsync(int orderId);
    }
}
