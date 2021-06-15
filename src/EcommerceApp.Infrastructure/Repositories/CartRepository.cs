using System;
using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _appDbContext;

        public CartRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task AddCartAsync(Cart cart)
        {
            await _appDbContext.AddAsync(cart);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<Cart> GetCartAsync(int cartId)
        {
            return await _appDbContext.Carts.FindAsync(cartId);
        }

        public async Task<int> GetCartIdAsync(int customerId)
        {
            var cart = await _appDbContext.Carts.FirstOrDefaultAsync(x => x.CustomerId == customerId);
            return cart.Id;
        }
    }
}
