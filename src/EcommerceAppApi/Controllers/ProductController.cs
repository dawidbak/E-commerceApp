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

        [HttpGet("Product/{id}")]
        public async Task<IActionResult> Product([FromRoute] int id)
        {
            return Ok(await _productService.GetProductDetailsForUserAsync(id));
        }
    }
}
