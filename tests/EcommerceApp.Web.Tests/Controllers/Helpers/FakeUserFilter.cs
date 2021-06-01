using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EcommerceApp.Web.Tests.Controllers.Helpers
{
    public class FakeUserFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            context.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, UserSettings.UserId),
                new Claim(ClaimTypes.Name, UserSettings.Name),
                new Claim(ClaimTypes.Email, UserSettings.UserEmail),
                new Claim("Admin", "True")
            }));
            await next();
        }
    }
}
