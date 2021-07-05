using System;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IProductService _productService;
        public CategoryController(IProductService productService)
        {
            _productService = productService;
        }
        public IActionResult Index()
        {

            return View();
        }

        public async Task<IActionResult> Products(string category)
        {
            if (string.IsNullOrEmpty(category))
            {
                return NotFound("You must pass correct category name");
            }
            return View(await _productService.GetProductsByCategoryNameAsync(category));
        }
    }
}
