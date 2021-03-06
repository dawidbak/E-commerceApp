using System;
using System.Security.Claims;
using System.Threading.Tasks;
using EcommerceApp.Application.Filters;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EcommerceApp.Web.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IConfiguration _configuration;

        public OrderController(IOrderService orderService, IConfiguration configuration)
        {
            _orderService = orderService;
            _configuration = configuration;
        }

        [HttpGet]
        [TypeFilter(typeof(CheckCheckoutGetPermission))]
        public async Task<IActionResult> Checkout(int? customerId)
        {
            if (!customerId.HasValue)
            {
                return NotFound("Can't redirect to checkout process");
            }
            return View(await _orderService.GetDataForOrderCheckoutAsync(customerId.Value));
        }

        [HttpPost]
        [TypeFilter(typeof(CheckCheckoutPostPermission))]
        public async Task<IActionResult> Checkout(OrderCheckoutVM orderCheckoutVM)
        {
            if (ModelState.IsValid)
            {
                await _orderService.AddOrderAsync(orderCheckoutVM);
                return RedirectToAction(nameof(CheckoutSuccessful));
            }
            return BadRequest();
        }

        public async Task<IActionResult> OrderHistory(int? pageNumber,string pageSize)
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
            return View(await _orderService.GetAllPaginatedCustomerOrdersAsync(intPageSize, pageNumber.Value, userId));
        }

        public async Task<IActionResult> OrderDetails(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound("You must pass a valid Customer ID in the route, for example, /Order/OrderDetails/21");
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(await _orderService.GetCustomerOrderDetailsAsync(id.Value, userId));
        }

        public ActionResult CheckoutSuccessful()
        {
            return View();
        }
    }
}
