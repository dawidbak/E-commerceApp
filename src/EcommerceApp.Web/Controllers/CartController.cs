using System;
using System.Security.Claims;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Web.Controllers
{
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
            return View(await _cartService.GetAllCartItemsForCurrentUser(userId));
        }

        public async Task<IActionResult> AddToCart(int id, int quantity)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await _cartService.AddCartItemAsync(id, quantity, userId);
            return RedirectToAction(nameof(Index));
        }
    }
}
