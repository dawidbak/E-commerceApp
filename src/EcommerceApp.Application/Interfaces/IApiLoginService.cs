using System;
using System.Threading.Tasks;
using EcommerceApp.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Application.Interfaces
{
    public interface IApiLoginService
    {
        Task<IActionResult> Login(UserModel userModel);
    }
}
