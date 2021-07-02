using System;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Product(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound("You must pass a valid Product ID in the route, for example, /Product/21");
            }
            return Ok(await _productService.GetProductDetailsForUserAsync(id.Value));
        }
    }
}
