using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.EmployeePanel;
using EcommerceApp.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EcommerceApp.Web.Tests.Controllers.UnitTests
{
    public class EmployeePanelControllerUnitTests
    {
        private readonly Mock<IProductService> _productService = new();
        private readonly Mock<ICategoryService> _categoryService = new();
        private readonly Mock<ISearchService> _searchService = new();
        private readonly Mock<ILogger<EmployeePanelController>> _logger = new();
        private readonly EmployeePanelController _sut;

        public EmployeePanelControllerUnitTests()
        {
            _sut = new EmployeePanelController(_productService.Object, _categoryService.Object, _logger.Object,_searchService.Object);
        }

        [Fact]
        public void Index_ReturnViewResult()
        {
            //Act
            var result = _sut.Index();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Categories_ReturnsCorrectViewResultWithListOfCategories()
        {
            //Arrange
            _categoryService.Setup(x => x.GetAllCategoriesAsync()).ReturnsAsync(GetCategoryVMs());

            //Act
            var results = await _sut.Categories(string.Empty,string.Empty);

            //Assert
            Assert.NotNull(results);
            var viewResult = Assert.IsType<ViewResult>(results);
            var model = Assert.IsAssignableFrom<List<CategoryVM>>(viewResult.Model);
            Assert.Equal(model[0].Id, GetCategoryVMs()[0].Id);
            Assert.Equal(model.Count, GetCategoryVMs().Count);
        }

        [Fact]
        public async Task Categories_ReturnsCorrectViewResultWithSearchedCategories()
        {
            //Arrange
            string selectedValue = "Name";
            string searchString = "abcd";
            _searchService.Setup(x => x.SearchSelectedCategoryAsync(selectedValue,searchString)).ReturnsAsync(GetCategoryVMs());

            //Act
            var results = await _sut.Categories(selectedValue,searchString);

            //Assert
            Assert.NotNull(results);
            var viewResult = Assert.IsType<ViewResult>(results);
            var model = Assert.IsAssignableFrom<List<CategoryVM>>(viewResult.Model);
            Assert.Equal(model[0].Id, GetCategoryVMs()[0].Id);
            Assert.Equal(model.Count, GetCategoryVMs().Count);
        }

        [Fact]
        public async Task Products_ReturnsCorrectViewResultWithListOfProducts()
        {
            //Arrange
            _productService.Setup(x => x.GetAllProductsAsync()).ReturnsAsync(GetProductVMs());

            //Act
            var results = await _sut.Products(string.Empty,string.Empty);

            //Assert
            Assert.NotNull(results);
            var viewResult = Assert.IsType<ViewResult>(results);
            var model = Assert.IsAssignableFrom<List<ProductVM>>(viewResult.Model);
            Assert.Equal(model[0].Name, GetProductVMs()[0].Name);
            Assert.Equal(model.Count, GetProductVMs().Count);
        }

        [Fact]
        public async Task Products_ReturnsCorrectViewResultWithSearchedProducts()
        {
            //Arrange
            string selectedValue = "Name";
            string searchString = "abcd";
            _searchService.Setup(x => x.SearchSelectedProductAsync(selectedValue,searchString)).ReturnsAsync(GetProductVMs());

            //Act
            var results = await _sut.Products(selectedValue,searchString);

            //Assert
            Assert.NotNull(results);
            var viewResult = Assert.IsType<ViewResult>(results);
            var model = Assert.IsAssignableFrom<List<ProductVM>>(viewResult.Model);
            Assert.Equal(model[0].Id, GetProductVMs()[0].Id);
            Assert.Equal(model.Count, GetProductVMs().Count);
        }

        [Fact]
        public void AddProduct_Get_ReturnsViewModel()
        {
            //Act
            var result = _sut.AddProduct();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void AddCategory_Get_ReturnsViewModel()
        {
            //Act
            var result = _sut.AddCategory();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task AddProduct_Post_ReturnsBadRequestWhenModelStateIsInvalid()
        {
            //Arrange
            var productVM = new ProductVM { Id = 1, Name = "abcd", Description = "abcd" };
            _sut.ModelState.AddModelError("BadModel", "ChangeModel");

            //Act
            var result = await _sut.AddProduct(productVM);

            //Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task AddCategory_Post_ReturnsBadRequestWhenModelStateIsInvalid()
        {
            //Arrange
            var categoryVM = new CategoryVM { Id = 1, Name = "abcd", Description = "abcd" };
            _sut.ModelState.AddModelError("BadModel", "ChangeModel");

            //Act
            var result = await _sut.AddCategory(categoryVM);

            //Assert
            Assert.IsType<BadRequestResult>(result);
        }


        private static List<CategoryVM> GetCategoryVMs()
        {
            //Arrange
            var categoryVM1 = new CategoryVM { Id = 1, Name = "abcd", Description = "abcd" };
            var categoryVM2 = new CategoryVM { Id = 2, Name = "test", Description = "test" };
            var categoryVM3 = new CategoryVM { Id = 3, Name = "good", Description = "good" };

            return new List<CategoryVM> { categoryVM1, categoryVM2, categoryVM3 };
        }

        private static List<ProductVM> GetProductVMs()
        {
            //Arrange
            var productVM1 = new ProductVM { Id = 1, Name = "abcd", Description = "abcd" };
            var productVM2 = new ProductVM { Id = 2, Name = "test", Description = "test" };
            var productVM3 = new ProductVM { Id = 3, Name = "good", Description = "good" };

            return new List<ProductVM> { productVM1, productVM2, productVM3 };
        }
    }
}
