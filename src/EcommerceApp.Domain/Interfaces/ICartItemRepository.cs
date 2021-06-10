using System;
using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Domain.Interfaces
{
    public interface ICartItemRepository
    {
        Task AddCartItemAsync(CartItem cartItem);
        Task DeleteCartItemAsync(int cartItemId);
        Task<IQueryable<CartItem>> GetAllCartItemsAsync();
        Task<CartItem> GetCartItemAsync(int cartItemId);
        Task UpdateCartItemAsync(CartItem cartItem);
    }
}
