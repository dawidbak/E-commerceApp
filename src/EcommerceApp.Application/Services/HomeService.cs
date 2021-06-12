using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.Home;
using EcommerceApp.Application.ViewModels.Product;

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
            var productsVM = await _productService.GetAllProductsWithImagesAsync();

            return new HomeVM()
            {
                Products = GetRandomProductVMList(productsVM),
            };
        }

        public ListProductDetailsForUserVM GetRandomProductVMList(ListProductDetailsForUserVM products)
        {
            var randomProductVMList = new ListProductDetailsForUserVM();
            var checkList = new List<int>();
            int numberOfProducts = 8;
            Random random = new();

            if (numberOfProducts >= products.Products.Count)
            {
                return products;
            }

            while (randomProductVMList.Products.Count <= numberOfProducts)
            {
                int index = random.Next(products.Products.Count);
                if (!checkList.Contains(index))
                {
                    randomProductVMList.Products.Add(products.Products[index]);
                    checkList.Add(index);
                }
            }

            return randomProductVMList;
        }
    }
}
