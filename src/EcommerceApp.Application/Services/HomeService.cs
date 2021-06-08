using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.EmployeePanel;
using EcommerceApp.Application.ViewModels.Home;

namespace EcommerceApp.Application.Services
{
    public class HomeService : IHomeService
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;

        public HomeService(ICategoryService categoryService, IProductService product)
        {
            _categoryService = categoryService;
            _productService = product;
        }
        public async Task<HomeVM> GetHomeVMForIndexAsync()
        {
            var categoriesVM = await _categoryService.GetAllCategoriesAsync();
            var productsVM = await _productService.GetAllProductsWithImagesAsync();
            
            HomeVM homeVM = new()
            {
                Categories = categoriesVM,
                Products = await GetRandomProductVMList(productsVM),
            };

            return homeVM;
        }

        public Task<List<ProductVM>> GetRandomProductVMList(List<ProductVM> products)
        {
            var randomProductVMList = new List<ProductVM>();
            var checkList = new List<int>();
            int numberOfProducts = 8;
            Random random = new();

            if (numberOfProducts >= products.Count)
            {
                return Task.FromResult(products);
            }

            while (randomProductVMList.Count <= numberOfProducts)
            {
                int index = random.Next(products.Count);
                if(!checkList.Contains(index))
                {
                    randomProductVMList.Add(products[index]);
                    checkList.Add(index);
                }
            }

            return Task.FromResult(randomProductVMList);
        }
    }
}
