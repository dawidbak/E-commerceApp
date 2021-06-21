using System;
using System.Security.Claims;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Web.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return View(await _cartService.GetAllCartItemsForCurrentUserAsync(userId));
        }

        public async Task<IActionResult> AddToCart(int id, int quantity)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await _cartService.AddCartItemAsync(id, quantity, userId);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> IncreaseQuantityCartItemByOne(int? cartItemId)
        {
            if (!cartItemId.HasValue)
            {
                return NotFound();
            }
            await _cartService.IncreaseQuantityCartItemByOneAsync(cartItemId.Value);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DecreaseQuantityCartItemByOne(int? cartItemId)
        {
            if (!cartItemId.HasValue)
            {
                return NotFound();
            }
            await _cartService.DecreaseQuantityCartItemByOneAsync(cartItemId.Value);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteCartItem(int? cartItemId)
        {
            if (!cartItemId.HasValue)
            {
                return NotFound();
            }
            await _cartService.DeleteCartItemAsync(cartItemId.Value);
            return RedirectToAction(nameof(Index));
        }
    }
}
