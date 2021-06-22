using System;
using System.Security.Claims;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EcommerceApp.Application.Filters
{
    public class CheckCheckoutPostPermission : Attribute, IAsyncAuthorizationFilter
    {
        private readonly ICustomerService _customerService;

        public CheckCheckoutPostPermission(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var appUserId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var customerId = context.HttpContext.Request.Form["customerId"].ToString();

            bool intParse = int.TryParse(customerId, out int parsedCartId);
            var getCustomerId = await _customerService.GetCustomerIdByAppUserIdAsync(appUserId);

            if (!intParse)
            {
                context.Result = new BadRequestResult();
            }
            if (intParse && getCustomerId != parsedCartId)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
