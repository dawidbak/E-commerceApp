using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EcommerceApp.Application.Filters;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EcommerceAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IConfiguration _configuration;

        public OrderController(IOrderService orderService, IConfiguration configuration)
        {
            _orderService = orderService;
            _configuration = configuration;
        }

        [HttpGet("Checkout/{customerId}")]
        [TypeFilter(typeof(CheckCheckoutGetPermission))]
        public async Task<IActionResult> Checkout([FromRoute] int customerId)
        {
            return Ok(await _orderService.GetDataForOrderCheckoutAsync(customerId));
        }

        [HttpPost("Checkout")]
        [TypeFilter(typeof(CheckCheckoutPostPermission))]
        public async Task<IActionResult> Checkout([FromBody] OrderCheckoutVM orderCheckoutVM)
        {
            await _orderService.AddOrderAsync(orderCheckoutVM);
            return Ok();
        }

        [HttpGet("OrderHistory")]
        public async Task<IActionResult> OrderHistory(int? pageNumber, string pageSize)
        {
            if (!pageNumber.HasValue)
            {
                pageNumber = 1;
            }
            if (!int.TryParse(pageSize, out int intPageSize))
            {
                intPageSize = _configuration.GetValue("DefaultPageSize", 10);
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok(await _orderService.GetAllPaginatedCustomerOrdersAsync(intPageSize, pageNumber.Value, userId));
        }

        [HttpGet("OrderDetails/{id}")]
        public async Task<IActionResult> OrderDetails([FromRoute] int id)
        {
            var userId = User.Claims.First(x => x.Type == "UserId").Value;
            return Ok(await _orderService.GetCustomerOrderDetailsAsync(id, userId));
        }
    }
}
