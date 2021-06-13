using System;
using System.Threading.Tasks;
using EcommerceApp.Application.ViewModels.Cart;

namespace EcommerceApp.Application.Interfaces
{
    public interface ICartService
    {
        Task AddCartItemAsync(int productId, int quantity, string appUserId);
        Task<ListCartItemForListVM> GetAllCartItemsForCurrentUser(string appUserId);
    }
}
