using System;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [Route("/Product/{id}")]
        public async Task<IActionResult> Product(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound("You must pass a valid Product ID in the route, for example, /Product/21");
            }
            return View(await _productService.GetProductDetailsForUser(id.Value));
        }
    }
}
