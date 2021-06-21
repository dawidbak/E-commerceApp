using System;
using System.Security.Claims;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.Order;
using EcommerceApp.Web.Filters;
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

        public async Task<IActionResult> OrderHistory(int? pageNumber)
        {
            if (!pageNumber.HasValue)
            {
                pageNumber = 1;
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int pageSize = _configuration.GetValue("DefaultPageSize", 10);
            return View(await _orderService.GetAllPaginatedCustomerOrdersAsync(pageSize, pageNumber.Value, userId));
        }

        public async Task<IActionResult> OrderDetails(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(await _orderService.GetCustomerOrderDetailsAsync(id.Value,userId));
        }

        public ActionResult CheckoutSuccessful()
        {
            return View();
        }
    }
}
