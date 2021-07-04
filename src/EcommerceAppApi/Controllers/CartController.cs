using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.Claims.First(x => x.Type == "UserId").Value;
            return Ok(await _cartService.GetAllCartItemsForCurrentUserAsync(userId));
        }

        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart(int id, int quantity)
        {
            var userId = User.Claims.First(x => x.Type == "UserId").Value;
            await _cartService.AddCartItemAsync(id, quantity, userId);
            return Ok("Added to Cart");
        }

        [HttpPut("IncreaseQuantityCartItemByOne")]
        public async Task<IActionResult> IncreaseQuantityCartItemByOne(int? cartItemId)
        {
            if (!cartItemId.HasValue)
            {
                return NotFound();
            }
            await _cartService.IncreaseQuantityCartItemByOneAsync(cartItemId.Value);
            return RedirectToAction(nameof(Index));
        }

        [HttpPut("DecreaseQuantityCartItemByOne")]
        public async Task<IActionResult> DecreaseQuantityCartItemByOne(int? cartItemId)
        {
            if (!cartItemId.HasValue)
            {
                return NotFound();
            }
            await _cartService.DecreaseQuantityCartItemByOneAsync(cartItemId.Value);
            return RedirectToAction(nameof(Index));
        }

        [HttpDelete("DeleteCartItem")]
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
