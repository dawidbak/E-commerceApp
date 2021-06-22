using System;
using System.Linq;
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

        [HttpGet("Checkout")]
        [TypeFilter(typeof(CheckCheckoutGetPermission))]
        public async Task<IActionResult> Checkout(int? customerId)
        {
            if (!customerId.HasValue)
            {
                return NotFound("Can't redirect to checkout process");
            }
            return Ok(await _orderService.GetDataForOrderCheckoutAsync(customerId.Value));
        }

        [HttpPost("Checkout")]
        [TypeFilter(typeof(CheckCheckoutPostPermission))]
        public async Task<IActionResult> Checkout([FromBody] OrderCheckoutVM orderCheckoutVM)
        {
            if (ModelState.IsValid)
            {
                await _orderService.AddOrderAsync(orderCheckoutVM);
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet("OrderHistory")]
        public async Task<IActionResult> OrderHistory(int? pageNumber)
        {
            if (!pageNumber.HasValue)
            {
                pageNumber = 1;
            }
            var userId = User.Claims.First(x => x.Type == "UserId").Value;
            int pageSize = _configuration.GetValue("DefaultPageSize", 10);
            return Ok(await _orderService.GetAllPaginatedCustomerOrdersAsync(pageSize, pageNumber.Value, userId));
        }

        [HttpGet("OrderDetails")]
        public async Task<IActionResult> OrderDetails(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }
            var userId = User.Claims.First(x => x.Type == "UserId").Value;
            return Ok(await _orderService.GetCustomerOrderDetailsAsync(id.Value, userId));
        }
    }
}
