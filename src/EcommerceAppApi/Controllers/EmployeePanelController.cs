using System;
using System.Net;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.EmployeePanel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EcommerceAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "EmployeePanelEntry")]
    public class EmployeePanelController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IOrderService _orderService;
        private readonly ISearchService _searchService;
        private readonly IConfiguration _configuration;

        public EmployeePanelController(IProductService productService, ICategoryService categoryService, ISearchService SearchService,
        IConfiguration configuration, IOrderService orderService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _searchService = SearchService;
            _configuration = configuration;
            _orderService = orderService;
        }

        [HttpGet("Categories")]
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
                return Ok(await _searchService.SearchSelectedCategoriesAsync(selectedValue, searchString, intPageSize, pageNumber.Value));
            }
            return Ok(await _categoryService.GetAllPaginatedCategoriesAsync(intPageSize, pageNumber.Value));
        }

        [HttpGet("Products")]
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
                return Ok(await _searchService.SearchSelectedProductsAsync(selectedValue, searchString, intPageSize, pageNumber.Value));
            }
            return Ok(await _productService.GetAllPaginatedProductsAsync(intPageSize, pageNumber.Value));
        }

        [HttpGet("Orders")]
        public async Task<IActionResult> Orders(string selectedValue, string searchString, string pageSize, int? pageNumber)
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
                return Ok(await _searchService.SearchSelectedOrdersAsync(selectedValue, searchString, intPageSize, pageNumber.Value));
            }
            return Ok(await _orderService.GetAllPaginatedOrdersAsync(intPageSize, pageNumber.Value));
        }

        [HttpGet("OrderDetails")]
        public async Task<IActionResult> OrderDetails(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound("You must pass a valid Order ID in the route, for example, /EmployeePanel/OrderDetails/21");
            }
            return Ok(await _orderService.GetOrderDetailsAsync(id.Value));
        }

        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct([FromBody] ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                await _productService.AddProductAsync(productVM);
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryVM categoryVM)
        {
            if (ModelState.IsValid)
            {
                await _categoryService.AddCategoryAsync(categoryVM);
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet("EditProduct")]
        public async Task<IActionResult> EditProduct(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound("You must pass a valid Product ID in the route, for example, /EmployeePanel/EditProduct/21");
            }
            return Ok(await _productService.GetProductAsync(id.Value));
        }

        [HttpPut("EditProduct")]
        public async Task<IActionResult> EditProduct([FromBody] ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                await _productService.UpdateProductAsync(productVM);
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet("EditCategory")]
        public async Task<IActionResult> EditCategory(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound("You must pass a valid Category ID in the route, for example, /EmployeePanel/EditCategory/21");
            }
            return Ok(await _categoryService.GetCategoryAsync(id.Value));
        }

        [HttpPut("EditCategory")]
        public async Task<IActionResult> EditCategory([FromBody] CategoryVM categoryVM)
        {
            if (ModelState.IsValid)
            {
                await _categoryService.UpdateCategoryAsync(categoryVM);
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound("You must pass a valid Product ID in the route, for example, /EmployeePanel/DeleteProduct/21");
            }
            await _productService.DeleteProductAsync(id.Value);
            return Ok();
        }

        [HttpDelete("DeleteCategory")]
        public async Task<IActionResult> DeleteCategory(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound("You must pass a valid Category ID in the route, for example, /EmployeePanel/DeleteCategory/21");
            }
            await _categoryService.DeleteCategoryAsync(id.Value);
            return Ok();
        }

        [HttpDelete("DeleteOrder")]
        public async Task<IActionResult> DeleteOrder(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound("You must pass a valid Order ID in the route, for example, /EmployeePanel/DeleteOrder/21");
            }
            await _orderService.DeleteOrderAsync(id.Value);
            return Ok();
        }
    }

}
