using System;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.Order;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> Checkout(int? cartId)
        {
            if (!cartId.HasValue)
            {
                return NotFound("Can't redirect to checkout process");
            }
            return View(await _orderService.GetDataForOrderCheckoutAsync(cartId.Value));
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(OrderCheckoutVM orderCheckoutVM)
        {
            if (ModelState.IsValid)
            {
                await _orderService.AddOrderAsync(orderCheckoutVM);
                return RedirectToAction(nameof(CheckoutSuccessful));
            }
            return BadRequest();
        }

        public ActionResult CheckoutSuccessful()
        {
            return View();
        }
    }
}
