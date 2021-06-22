using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EcommerceApp.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EcommerceAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public LoginController(IConfiguration configuration, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserModel userModel)
        {
            IActionResult result = Unauthorized();
            var success = AuthenticateUser(userModel);
            if (success)
            {
                var tokenStr = await GenetareJSONWebToken(userModel);
                result = Ok(new { token = tokenStr });
            }
            return result;
        }

        private async Task<string> GenetareJSONWebToken([FromBody] UserModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var user = await _userManager.FindByNameAsync(userInfo.Email);
            var claims = await _userManager.GetClaimsAsync(user);
            var claim = new Claim(string.Empty, string.Empty);

            foreach (var item in claims)
            {
                if (item.Type == "Admin" && item.Value == "True")
                {
                    claim = item;
                }
                else if (item.Type == "isEmployee" && item.Value == "True")
                {
                    claim = item;
                }
            }

            var claimsToToken = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(claim.Type, claim.Value),
                new Claim("UserId", user.Id)
            };

            var token = new JwtSecurityToken(_configuration["JWT:Issuer"], _configuration["JWT:Issuer"], claimsToToken, expires: DateTime.Now.AddMinutes(180), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private bool AuthenticateUser(UserModel userModel)
        {
            var result = _signInManager.PasswordSignInAsync(userModel.Email, userModel.Password, true, false).Result;
            return result.Succeeded;
        }
    }
}
