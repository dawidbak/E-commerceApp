using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.EmployeePanel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EcommerceApp.Web.Controllers
{
    [Authorize(Policy = "EmployeePanelEntry")]
    public class EmployeePanelController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ILogger<EmployeePanelController> _logger;
        private readonly ISearchService _searchService;
        private readonly IConfiguration _configuration;

        public EmployeePanelController(IProductService productService, ICategoryService categoryService, ILogger<EmployeePanelController> logger, ISearchService SearchService,
        IConfiguration configuration)
        {
            _productService = productService;
            _categoryService = categoryService;
            _logger = logger;
            _searchService = SearchService;
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Categories(string selectedValue, string searchString, string pageSize, int? pageNumber)
        {
            if (!pageNumber.HasValue)
            {
                pageNumber = 1;
            }
            if (!int.TryParse(pageSize, out int intPageSize))
            {
                intPageSize = _configuration.GetValue("DefaultPageSize", 10);
            }

            if (!string.IsNullOrEmpty(selectedValue) && !string.IsNullOrEmpty(searchString))
            {
                return View(await _searchService.SearchSelectedCategoryAsync(selectedValue, searchString, intPageSize, pageNumber.Value));
            }
            return View(await _categoryService.GetAllPaginatedCategoriesAsync(intPageSize, pageNumber.Value));
        }

        public async Task<IActionResult> Products(string selectedValue, string searchString, string pageSize, int? pageNumber)
        {
            if (!pageNumber.HasValue)
            {
                pageNumber = 1;
            }
            if (!int.TryParse(pageSize, out int intPageSize))
            {
                intPageSize = _configuration.GetValue("DefaultPageSize", 10);
            }

            if (!string.IsNullOrEmpty(selectedValue) && !string.IsNullOrEmpty(searchString))
            {
                return View(await _searchService.SearchSelectedProductAsync(selectedValue, searchString, intPageSize, pageNumber.Value));
            }
            return View(await _productService.GetAllPaginatedProductsAsync(intPageSize, pageNumber.Value));
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
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
