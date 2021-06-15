using System;
using System.Threading.Tasks;
using EcommerceApp.Application.ViewModels.Order;

namespace EcommerceApp.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderCheckoutVM> GetDataForOrderCheckoutAsync(int cartId);
        Task AddOrderAsync(OrderCheckoutVM orderCheckoutVM);
    }
}
