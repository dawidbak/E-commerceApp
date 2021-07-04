using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.EmployeePanel;
using EcommerceApp.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EcommerceApp.Web.Tests.Controllers.UnitTests
{
    public class EmployeePanelControllerUnitTests
    {
        private readonly Mock<IProductService> _productService = new();
        private readonly Mock<ICategoryService> _categoryService = new();
        private readonly Mock<IOrderService> _orderService = new();
        private readonly Mock<IConfiguration> _configuration = new();
        private readonly Mock<ISearchService> _searchService = new();
        private readonly Mock<ILogger<EmployeePanelController>> _logger = new();
        private readonly EmployeePanelController _sut;

        public EmployeePanelControllerUnitTests()
        {
            _sut = new EmployeePanelController
            (
                _productService.Object,
                _categoryService.Object,
                _logger.Object,
                _searchService.Object,
                _configuration.Object,
                _orderService.Object
            );
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
        #region Categories,Products,Orders
        [Fact]
        public async Task Categories_ReturnsCorrectViewResultWithAllCategories()
        {
            //Arrange
            var categoryForListVMs = new List<CategoryForListVM>
            {
                new CategoryForListVM{Id = 1, Name = "test"},
                new CategoryForListVM{Id = 2, Name = "unit"}
            };
            var listCategoryForListVM = new ListCategoryForListVM
            {
                Categories = categoryForListVMs,
                TotalPages = 1,
                CurrentPage = 1,
            };

            _categoryService.Setup(x => x.GetAllPaginatedCategoriesAsync(10, 1)).ReturnsAsync(listCategoryForListVM);

            //Act
            var results = await _sut.Categories(string.Empty, string.Empty, "10", 1);

            //Assert
            Assert.NotNull(results);
            var viewResult = Assert.IsType<ViewResult>(results);
            var model = Assert.IsAssignableFrom<ListCategoryForListVM>(viewResult.Model);
            Assert.Equal(model.Categories, listCategoryForListVM.Categories);
        }

        [Fact]
        public async Task Categories_ReturnsCorrectViewResultWithSearchedCategories()
        {
            //Arrange
            var categoryForListVMs = new List<CategoryForListVM>
            {
                new CategoryForListVM{Id = 2, Name = "unit"}
            };
            var listCategoryForListVM = new ListCategoryForListVM
            {
                Categories = categoryForListVMs,
                TotalPages = 1,
                CurrentPage = 1,
            };
            string selectedValue = "Name";
            string searchString = "unit";

            _searchService.Setup(x => x.SearchSelectedCategoriesAsync(selectedValue, searchString, 10, 1)).ReturnsAsync(listCategoryForListVM);

            //Act
            var results = await _sut.Categories(selectedValue, searchString, "10", 1);

            //Assert
            Assert.NotNull(results);
            var viewResult = Assert.IsType<ViewResult>(results);
            var model = Assert.IsAssignableFrom<ListCategoryForListVM>(viewResult.Model);
            Assert.Equal(model.Categories, listCategoryForListVM.Categories);
        }

        [Fact]
        public async Task Products_ReturnsCorrectViewResultWithAllProducts()
        {
            //Arrange
            var productForListVMs = new List<ProductForListVM>
            {
                new ProductForListVM{Id = 2, Name = "unit", UnitPrice = 1.1m},
                new ProductForListVM{Id = 1, Name = "test", UnitPrice = 22.1m}
            };
            var listProductForListVM = new ListProductForListVM
            {
                Products = productForListVMs,
                TotalPages = 1,
                CurrentPage = 1,
            };

            _productService.Setup(x => x.GetAllPaginatedProductsAsync(10, 1)).ReturnsAsync(listProductForListVM);

            //Act
            var results = await _sut.Products(string.Empty, string.Empty, "10", 1);

            //Assert
            Assert.NotNull(results);
            var viewResult = Assert.IsType<ViewResult>(results);
            var model = Assert.IsAssignableFrom<ListProductForListVM>(viewResult.Model);
            Assert.Equal(model.Products, listProductForListVM.Products);
        }

        [Fact]
        public async Task Products_ReturnsCorrectViewResultWithSearchedProducts()
        {
            //Arrange
            var productForListVMs = new List<ProductForListVM>
            {
                new ProductForListVM{Id = 1, Name = "test", UnitPrice = 22.1m}
            };
            var listProductForListVM = new ListProductForListVM
            {
                Products = productForListVMs,
                TotalPages = 1,
                CurrentPage = 1,
            };
            string selectedValue = "Name";
            string searchString = "test";
            _searchService.Setup(x => x.SearchSelectedProductsAsync(selectedValue, searchString, 10, 1)).ReturnsAsync(listProductForListVM);

            //Act
            var results = await _sut.Products(selectedValue, searchString, "10", 1);

            //Assert
            Assert.NotNull(results);
            var viewResult = Assert.IsType<ViewResult>(results);
            var model = Assert.IsAssignableFrom<ListProductForListVM>(viewResult.Model);
            Assert.Equal(model.Products, listProductForListVM.Products);
        }

        [Fact]
        public async Task Orders_ReturnsCorrectViewResultWithAllOrders()
        {
            //Arrange
            var orderForListVMs = new List<OrderForListVM>
            {
                new OrderForListVM{Id = 2, CustomerId = 1, Price = 11},
                new OrderForListVM{Id = 1, CustomerId = 2, Price = 22.1m}
            };
            var listOrderForListVM = new ListOrderForListVM
            {
                Orders = orderForListVMs,
                TotalPages = 1,
                CurrentPage = 1,
            };

            _orderService.Setup(x => x.GetAllPaginatedOrdersAsync(10, 1)).ReturnsAsync(listOrderForListVM);

            //Act
            var results = await _sut.Orders(string.Empty, string.Empty, "10", 1);

            //Assert
            Assert.NotNull(results);
            var viewResult = Assert.IsType<ViewResult>(results);
            var model = Assert.IsAssignableFrom<ListOrderForListVM>(viewResult.Model);
            Assert.Equal(model.Orders, listOrderForListVM.Orders);
        }

        [Fact]
        public async Task Orders_ReturnsCorrectViewResultWithSearchedOrders()
        {
            //Arrange
            var orderForListVMs = new List<OrderForListVM>
            {
                new OrderForListVM{Id = 1, CustomerId = 2, Price = 22.1m}
            };
            var listOrderForListVM = new ListOrderForListVM
            {
                Orders = orderForListVMs,
                TotalPages = 1,
                CurrentPage = 1,
            };
            string selectedValue = "CustomerId";
            string searchString = "2";
            _searchService.Setup(x => x.SearchSelectedOrdersAsync(selectedValue, searchString, 10, 1)).ReturnsAsync(listOrderForListVM);

            //Act
            var results = await _sut.Orders(selectedValue, searchString, "10", 1);

            //Assert
            Assert.NotNull(results);
            var viewResult = Assert.IsType<ViewResult>(results);
            var model = Assert.IsAssignableFrom<ListOrderForListVM>(viewResult.Model);
            Assert.Equal(model.Orders, listOrderForListVM.Orders);
        }
        #endregion

        #region OrderDetails
        [Fact]
        public async Task OrderDetails_ReturnsNotFoundWhenIdIsNull()
        {
            //Act
            var result = await _sut.OrderDetails(id: null);

            //Assert
            var redirectToNotFoundPage = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, redirectToNotFoundPage.StatusCode);
        }

        [Fact]
        public async Task OrderDetails_ReturnsViewResult()
        {
            //Assert
            var orderDetailsVM = new OrderDetailsVM { Id = 1, Price = 11 };

            _orderService.Setup(x => x.GetOrderDetailsAsync(orderDetailsVM.Id)).ReturnsAsync(orderDetailsVM);

            //Act
            var result = await _sut.OrderDetails(orderDetailsVM.Id);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<OrderDetailsVM>(viewResult.Model);
            Assert.Equal(orderDetailsVM.Price, model.Price);
        }
        #endregion

        #region AddProduct, AddCategory
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
            var productVM = new ProductVM { };
            _sut.ModelState.AddModelError("BadModel", "ChangeModel");

            //Act
            var result = await _sut.AddProduct(productVM);

            //Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task AddProduct_Post_ReturnsRedirectToActionWhenModelStateIsValid()
        {
            //Arrange
            var productVM = new ProductVM { Id = 1, Name = "abcd", Description = "abcd" };

            //Act
            var result = await _sut.AddProduct(productVM);

            //Assert
            _productService.Verify(x => x.AddProductAsync(It.IsAny<ProductVM>()), Times.Once);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Products", redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task AddCategory_Post_ReturnsBadRequestWhenModelStateIsInvalid()
        {
            //Arrange
            var categoryVM = new CategoryVM { };
            _sut.ModelState.AddModelError("BadModel", "ChangeModel");

            //Act
            var result = await _sut.AddCategory(categoryVM);

            //Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task AddCategory_Post_ReturnsRedirectToActionWhenModelStateIsValid()
        {
            //Arrange
            var categoryVM = new CategoryVM { Id = 1, Name = "abcd", Description = "abcd" };

            //Act
            var result = await _sut.AddCategory(categoryVM);

            //Assert
            _categoryService.Verify(x => x.AddCategoryAsync(It.IsAny<CategoryVM>()),Times.Once);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Categories", redirectToActionResult.ActionName);
        }
        #endregion

        #region EditProduct, EditCategory
        [Fact]
        public async Task EditProduct_Get_ReturnsNotFoundWhenIdIsNull()
        {
            //Act
            var result = await _sut.EditProduct(id: null);

            //Assert
            var redirectToNotFoundPage = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, redirectToNotFoundPage.StatusCode);
        }

        [Fact]
        public async Task EditProduct_Get_ReturnsViewResult()
        {
            //Assert
            var productVM = new ProductVM { Id = 1, Name = "test" };

            _productService.Setup(x => x.GetProductAsync(productVM.Id)).ReturnsAsync(productVM);

            //Act
            var result = await _sut.EditProduct(productVM.Id);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ProductVM>(viewResult.Model);
            Assert.Equal(productVM.Name, model.Name);
        }

        [Fact]
        public async Task EditCategory_Get_ReturnsNotFoundWhenIdIsNull()
        {
            //Act
            var result = await _sut.EditCategory(id: null);

            //Assert
            var redirectToNotFoundPage = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, redirectToNotFoundPage.StatusCode);
        }

        [Fact]
        public async Task EditCategory_Get_ReturnsViewResult()
        {
            //Assert
            var categoryVM = new CategoryVM { Id = 1, Name = "test" };

            _categoryService.Setup(x => x.GetCategoryAsync(categoryVM.Id)).ReturnsAsync(categoryVM);

            //Act
            var result = await _sut.EditCategory(categoryVM.Id);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CategoryVM>(viewResult.Model);
            Assert.Equal(categoryVM.Name, model.Name);
        }

        [Fact]
        public async Task EditProduct_Post_ReturnsBadRequestResultWhenModelStateIsInvalid()
        {
            //Arrange
            var productVM = new ProductVM { Id = 1, Name = "test", Description = "xdd", UnitPrice = 1, UnitsInStock = 23 };
            _sut.ModelState.AddModelError("ProductVM", "Invalid");

            //Act
            var result = await _sut.EditProduct(productVM);

            //Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task EditProduct_Post_ReturnsARedirectAndUpdatesProductWhenModelStateIsValid()
        {
            //Arrange
            var productVM = new ProductVM { Id = 1, Name = "test", Description = "xdd", UnitPrice = 1, UnitsInStock = 23 };

            //Act
            var result = await _sut.EditProduct(productVM);

            //Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Products", redirectToActionResult.ActionName);
            _productService.Verify(x => x.UpdateProductAsync(It.IsAny<ProductVM>()), Times.Once);
        }

        [Fact]
        public async Task EditCategory_Post_ReturnsBadRequestResultWhenModelStateIsInvalid()
        {
            //Arrange
            var categoryVM = new CategoryVM { Id = 1, Name = "test", Description = "asdas" };
            _sut.ModelState.AddModelError("CategoryVM", "Invalid");

            //Act
            var result = await _sut.EditCategory(categoryVM);

            //Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task EditCategory_Post_ReturnsARedirectAndUpdatesCategoryWhenModelStateIsValid()
        {
            //Arrange
            var categoryVM = new CategoryVM { Id = 1, Name = "test", Description = "asdas" };

            //Act
            var result = await _sut.EditCategory(categoryVM);

            //Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Categories", redirectToActionResult.ActionName);
            _categoryService.Verify(x => x.UpdateCategoryAsync(It.IsAny<CategoryVM>()), Times.Once);
        }
        #endregion

        #region DeleteProduct, DeleteCategory, DeleteOrder
        [Fact]
        public async Task DeleteProduct_ReturnsNotFoundWhenIdIsNull()
        {
            //Act
            var result = await _sut.DeleteProduct(id: null);

            //Assert
            var redirectToNotFoundPage = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, redirectToNotFoundPage.StatusCode);
        }

        [Fact]
        public async Task DeleteProduct_DeleteProductAndRedirectToActionWhenIdHasValue()
        {
            //Act
            var result = await _sut.DeleteProduct(1);

            //Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Products", redirectToActionResult.ActionName);
            _productService.Verify(x => x.DeleteProductAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task DeleteCategory_ReturnsNotFoundWhenIdIsNull()
        {
            //Act
            var result = await _sut.DeleteCategory(id: null);

            //Assert
            var redirectToNotFoundPage = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, redirectToNotFoundPage.StatusCode);
        }

        [Fact]
        public async Task DeleteCategory_DeleteCategoryAndRedirectToActionWhenIdHasValue()
        {
            //Act
            var result = await _sut.DeleteCategory(1);

            //Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Categories", redirectToActionResult.ActionName);
            _categoryService.Verify(x => x.DeleteCategoryAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task DeleteOrder_ReturnsNotFoundWhenIdIsNull()
        {
            //Act
            var result = await _sut.DeleteOrder(id: null);

            //Assert
            var redirectToNotFoundPage = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, redirectToNotFoundPage.StatusCode);
        }

        [Fact]
        public async Task DeleteOrder_DeleteOrderAndRedirectToActionWhenIdHasValue()
        {
            //Act
            var result = await _sut.DeleteOrder(1);

            //Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Orders", redirectToActionResult.ActionName);
            _orderService.Verify(x => x.DeleteOrderAsync(It.IsAny<int>()), Times.Once);
        }
        #endregion
    }
}
