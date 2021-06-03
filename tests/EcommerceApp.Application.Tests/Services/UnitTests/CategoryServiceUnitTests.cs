using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EcommerceApp.Application.Services;
using EcommerceApp.Application.ViewModels.EmployeePanel;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Moq;
using Xunit;
namespace EcommerceApp.Application.Tests.Services.UnitTests
{
    public class CategoryServiceUnitTests
    {
        private readonly CategoryService _sut;
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<ICategoryRepository> _categoryRepository = new Mock<ICategoryRepository>();

        public CategoryServiceUnitTests()
        {
            _sut = new CategoryService(_categoryRepository.Object, _mapper.Object);
        }

        [Fact]
        public async Task AddCategoryAsync_ShouldCategoryExistsAfterAdd()
        {
            //Arrange
            var category = new Category() { CategoryId = 10, Name = "GoodCategory", Description = "test" };
            var categoryVM = new CategoryVM() { CategoryId = 10, Name = "GoodCategory", Description = "test" };

            _mapper.Setup(x => x.Map<Category>(categoryVM)).Returns(category);

            //Act
            await _sut.AddCategoryAsync(categoryVM);

            //Assert
            _categoryRepository.Verify(c => c.AddCategoryAsync(It.IsAny<Category>()), Times.Once);
            Assert.Equal(category.CategoryId, categoryVM.CategoryId);
            Assert.Equal(category.Name, categoryVM.Name);
            Assert.Equal(category.Description, categoryVM.Description);
        }

        [Fact]
        public async Task GetCategoryAsync_ShouldReturnCategoryVMAndBeEqualToTheModel()
        {
            //Arrange
            var category = new Category() { CategoryId = 10, Name = "GoodCategory", Description = "test" };
            var categoryVM = new CategoryVM() { CategoryId = 10, Name = "GoodCategory", Description = "test" };

            _categoryRepository.Setup(x => x.GetCategoryAsync(category.CategoryId)).ReturnsAsync(category);
            _mapper.Setup(x => x.Map<CategoryVM>(category)).Returns(categoryVM);

            //Act
            var result = await _sut.GetCategoryAsync(categoryVM.CategoryId);

            //Arrange
            Assert.Equal(category.CategoryId, result.CategoryId);
            Assert.Equal(category.Name, result.Name);
            Assert.Equal(category.Description, result.Description);
        }

        [Fact]
        public async Task GetAllCategoriesAsync_GetListOfCategoriesAndCheckAreEqualLikeModels()
        {
            //Arrange
            var category1 = new Category() { CategoryId = 10, Name = "GoodCategory", Description = "test" };
            var category2 = new Category() { CategoryId = 11, Name = "BadCategory", Description = "xtest" };
            List<Category> categories = new() { category1, category2 };
            var categoryVM1 = new CategoryVM() { CategoryId = 10, Name = "GoodCategory", Description = "test" };
            var categoryVM2 = new CategoryVM() { CategoryId = 11, Name = "BadCategory", Description = "xtest" };
            List<CategoryVM> categoriesVM = new() { categoryVM1, categoryVM2 };

            _categoryRepository.Setup(x => x.GetAllCategoriesAsync()).ReturnsAsync(categories.AsQueryable);
            _mapper.Setup(x => x.Map<List<CategoryVM>>(categories)).Returns(categoriesVM);
            //Act
            var results = await _sut.GetAllCategoriesAsync();

            //Assert
            for (int i = 0; i < results.Count; i++)
            {
                Assert.Equal(categories[i].CategoryId, results[i].CategoryId);
                Assert.Equal(categories[i].Name, results[i].Name);
                Assert.Equal(categories[i].Description, results[i].Description);
            }
        }

        [Fact]
        public async Task UpdateCategoryAsync_ShouldRunsUpdateOnce()
        {
            //Arrange
            var category = new Category() { CategoryId = 10, Name = "GoodCategory", Description = "test" };
            var categoryVM = new CategoryVM() { CategoryId = 10, Name = "BadCategory", Description = "xtest" };

            _mapper.Setup(x => x.Map<Category>(categoryVM)).Returns(category);
            //Act
            await _sut.UpdateCategoryAsync(categoryVM);
            //Assert
            _categoryRepository.Verify(x => x.UpdateCategoryAsync(It.IsAny<Category>()), Times.Once);
            Assert.Equal(category.CategoryId, categoryVM.CategoryId);
        }

        [Fact]
        public async Task DeleteCategoryAsync_ShouldDeleteMethodRunsOnce()
        {
            //Arrange
            var category = new Category() { CategoryId = 10 };

            //Act
            await _sut.DeleteCategoryAsync(category.CategoryId);

            //Assert
            _categoryRepository.Verify(x => x.DeleteCategoryAsync(It.IsAny<int>()), Times.Once);
        }
    }
}
