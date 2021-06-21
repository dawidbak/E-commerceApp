using System;
using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Domain.Interfaces
{
    public interface ICartRepository
    {
        Task AddCartAsync(Cart cart);
        IQueryable<Cart> GetAllCarts();
        Task<int> GetCartIdAsync(int customerId);
        Task<Cart> GetCartAsync(int cartId);
    }
}
