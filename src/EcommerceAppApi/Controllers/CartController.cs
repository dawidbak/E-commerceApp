using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var userId = User.Claims.First(x => x.Type == "UserId").Value;
            return Ok(await _cartService.GetAllCartItemsForCurrentUserAsync(userId));
        }

        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart([FromQuery] int? id, [FromQuery] int? quantity)
        {
            if (!id.HasValue || !quantity.HasValue)
            {
                return NotFound("You must pass a valid ID and quantity in the route");
            }
            var userId = User.Claims.First(x => x.Type == "UserId").Value;
            await _cartService.AddCartItemAsync(id.Value, quantity.Value, userId);
            return Ok("Added to Cart");
        }

        [HttpPut("IncreaseQuantityCartItemByOne/{cartItemId}")]
        public async Task<IActionResult> IncreaseQuantityCartItemByOne([FromRoute] int? cartItemId)
        {
            await _cartService.IncreaseQuantityCartItemByOneAsync(cartItemId.Value);
            return Ok();
        }

        [HttpPut("DecreaseQuantityCartItemByOne/{cartItemId}")]
        public async Task<IActionResult> DecreaseQuantityCartItemByOne([FromRoute] int? cartItemId)
        {
            await _cartService.DecreaseQuantityCartItemByOneAsync(cartItemId.Value);
            return Ok();
        }

        [HttpDelete("DeleteCartItem/{cartItemId}")]
        public async Task<IActionResult> DeleteCartItem([FromRoute] int? cartItemId)
        {
            await _cartService.DeleteCartItemAsync(cartItemId.Value);
            return Ok();
        }
    }
}
