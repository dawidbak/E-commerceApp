using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly IApiLoginService _apiLoginService;
        public LoginController(IApiLoginService apiLoginService)
        {
            _apiLoginService = apiLoginService;
        }

        public async Task<IActionResult> Login([FromBody] UserModel userModel)
        {
            return await _apiLoginService.Login(userModel);
        }
    }
}
