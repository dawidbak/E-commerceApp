using System;
using System.Security.Claims;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EcommerceApp.Web.Filters
{
    public class CheckCheckoutPostPermission : Attribute, IAsyncAuthorizationFilter
    {
        private readonly ICartService _cartService;

        public CheckCheckoutPostPermission(ICartService cartService)
        {
            _cartService = cartService;
        }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var appUserId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var cartId = context.HttpContext.Request.Form["CartId"].ToString();

            bool intParse = int.TryParse(cartId, out int parsedCartId);
            var getCartId = await _cartService.GetCartIdAsync(appUserId);

            if(!intParse)
            {
                context.Result = new BadRequestResult();
            }
            if (intParse && getCartId != parsedCartId)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
