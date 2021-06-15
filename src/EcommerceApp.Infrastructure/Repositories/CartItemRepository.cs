using System;
using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Infrastructure.Repositories
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly AppDbContext _appDbContext;
        public CartItemRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task AddCartItemAsync(CartItem cartItem)
        {
            await _appDbContext.AddAsync(cartItem);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteCartItemAsync(int cartItemId)
        {
            var cartItem = await _appDbContext.CartItems.FindAsync(cartItemId);
            if (cartItem != null)
            {
                _appDbContext.CartItems.Remove(cartItem);
                await _appDbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAllCartItemsByCartIdAsync(int cartId)
        {
            var cartItems = await _appDbContext.CartItems.Where(x => x.CartId == cartId).ToListAsync();
            _appDbContext.CartItems.RemoveRange(cartItems);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<IQueryable<CartItem>> GetAllCartItemsByCartIdAsync(int cartId)
        {
            return (await _appDbContext.CartItems.Where(x => x.CartId == cartId).ToListAsync()).AsQueryable();
        }

        public async Task<IQueryable<CartItem>> GetAllCartItemsAsync()
        {
            return (await _appDbContext.CartItems.ToListAsync()).AsQueryable();
        }

        public async Task<CartItem> GetCartItemAsync(int cartItemId)
        {
            return await _appDbContext.CartItems.FindAsync(cartItemId);
        }

        public async Task UpdateCartItemAsync(CartItem cartItem)
        {
            _appDbContext.CartItems.Update(cartItem);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
