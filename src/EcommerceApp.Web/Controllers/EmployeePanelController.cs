using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.EmployeePanel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EcommerceApp.Web.Controllers
{
    [Authorize("Admin,Employee")]
    public class EmployeePanelController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ILogger _logger;

        public EmployeePanelController(IProductService productService, ICategoryService categoryService, ILogger logger)
        {
            _productService = productService;
            _categoryService = categoryService;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Categories()
        {
            var model = await _categoryService.GetAllCategoriesAsync();
            return View(model);
        }

        public async Task<IActionResult> Products()
        {
            var model = await _productService.GetAllProductsAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddProduct()
        {
            var model = await _categoryService.GetAllCategoriesAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                await _productService.AddProductAsync(productVM);
                return RedirectToAction(nameof(Products));
            }
            return BadRequest();
        }

        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(CategoryVM categoryVM)
        {
            if (ModelState.IsValid)
            {
                await _categoryService.AddCategoryAsync(categoryVM);
                return RedirectToAction(nameof(Categories));
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> EditProduct(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound("You must pass a valid Product ID in the route, for example, /EmployeePanel/EditProduct/21");
            }
            var model = await _productService.GetProductAsync(id.Value);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                await _productService.UpdateProductAsync(productVM);
                return RedirectToAction(nameof(Products));
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> EditCategory(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound("You must pass a valid Product ID in the route, for example, /EmployeePanel/EditCategory/21");
            }
            var model = await _categoryService.GetCategoryAsync(id.Value);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditCategory(CategoryVM categoryVM)
        {
            if (ModelState.IsValid)
            {
                await _categoryService.UpdateCategoryAsync(categoryVM);
                return RedirectToAction(nameof(Categories));
            }
            return BadRequest();
        }

        public async Task<IActionResult> DeleteProduct(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound("You must pass a valid Product ID in the route, for example, /EmployeePanel/DeleteProduct/21");
            }
            await _productService.DeleteProductAsync(id.Value);
            return RedirectToAction(nameof(Products));
        }

        public async Task<IActionResult> DeleteCategory(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound("You must pass a valid Product ID in the route, for example, /EmployeePanel/DeleteCategory/21");
            }
            await _categoryService.DeleteCategoryAsync(id.Value);
            return RedirectToAction(nameof(Categories));
        }
    }
}
