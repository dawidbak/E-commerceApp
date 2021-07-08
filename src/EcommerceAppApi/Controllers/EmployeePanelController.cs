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

        [HttpGet("OrderDetails/{id}")]
        public async Task<IActionResult> OrderDetails([FromRoute] int id)
        {
            return Ok(await _orderService.GetOrderDetailsAsync(id));
        }

        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct([FromBody] ProductVM productVM)
        {
            await _productService.AddProductAsync(productVM);
            return Ok();
        }

        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryVM categoryVM)
        {
            await _categoryService.AddCategoryAsync(categoryVM);
            return Ok();
        }

        [HttpGet("EditProduct/{id}")]
        public async Task<IActionResult> EditProduct([FromRoute] int id)
        {
            return Ok(await _productService.GetProductAsync(id));
        }

        [HttpPut("EditProduct")]
        public async Task<IActionResult> EditProduct([FromBody] ProductVM productVM)
        {
            await _productService.UpdateProductAsync(productVM);
            return Ok();
        }

        [HttpGet("EditCategory/{id}")]
        public async Task<IActionResult> EditCategory([FromRoute] int id)
        {
            return Ok(await _categoryService.GetCategoryAsync(id));
        }

        [HttpPut("EditCategory")]
        public async Task<IActionResult> EditCategory([FromBody] CategoryVM categoryVM)
        {
            await _categoryService.UpdateCategoryAsync(categoryVM);
            return Ok();
        }

        [HttpDelete("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            await _productService.DeleteProductAsync(id);
            return Ok();
        }

        [HttpDelete("DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] int id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            return Ok();
        }

        [HttpDelete("DeleteOrder/{id}")]
        public async Task<IActionResult> DeleteOrder([FromRoute] int id)
        {
            await _orderService.DeleteOrderAsync(id);
            return Ok();
        }
    }
}
