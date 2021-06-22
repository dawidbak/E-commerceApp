using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.Home;
using EcommerceApp.Application.ViewModels.Product;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Application.Services
{
    public class HomeService : IHomeService
    {
        private readonly IProductService _productService;

        public HomeService(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<HomeVM> GetHomeVMForIndexAsync()
        {
            return new HomeVM()
            {
                Products = await _productService.GetRandomProductsWithImagesAsync(8)
            };
        }
    }
}
