using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.Services;
using EcommerceApp.Application.ViewModels.EmployeePanel;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;
namespace EcommerceApp.Application.Tests.Services.UnitTests
{
    public class CategoryServiceUnitTests
    {
        private readonly CategoryService _sut;
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<ICategoryRepository> _categoryRepository = new Mock<ICategoryRepository>();
        private readonly Mock<IImageConverterService> _imageConverterService = new ();
        private readonly Mock<IFormFile> _fileMock = new();

        public CategoryServiceUnitTests()
        {
            _sut = new CategoryService(_categoryRepository.Object, _mapper.Object, _imageConverterService.Object);
        }

        [Fact]
        public async Task AddCategoryAsync_ShouldCategoryAddMethodRunsOnce()
        {
            //Arrange
            var category = new Category() { Id = 10, Name = "GoodCategory", Description = "test",Image = new byte[]{1,2,3} };
            var categoryVM = new CategoryVM() { Id = 10, Name = "GoodCategory", Description = "test" };

            _mapper.Setup(x => x.Map<Category>(categoryVM)).Returns(category);
            _imageConverterService.Setup(x => x.GetByteArrayFromImageAsync(_fileMock.Object)).ReturnsAsync(category.Image);

            //Act
            await _sut.AddCategoryAsync(categoryVM);

            //Assert
            _categoryRepository.Verify(c => c.AddCategoryAsync(It.IsAny<Category>()), Times.Once);
            _imageConverterService.Verify(x => x.GetByteArrayFromImageAsync(It.IsAny<IFormFile>()), Times.Once);
        }

        [Fact]
        public async Task GetCategoryAsync_ShouldReturnCategoryVMAndBeEqualToTheModel()
        {
            //Arrange
            var category = new Category() { Id = 10, Name = "GoodCategory", Description = "test",Image = new byte[]{1,2,3} };
            var categoryVM = new CategoryVM() { Id = 10, Name = "GoodCategory", Description = "test", ImageUrl ="test123" };

            _categoryRepository.Setup(x => x.GetCategoryAsync(category.Id)).ReturnsAsync(category);
            _mapper.Setup(x => x.Map<CategoryVM>(category)).Returns(categoryVM);
            _imageConverterService.Setup(x => x.GetImageUrlFromByteArray(category.Image)).Returns(categoryVM.ImageUrl);

            //Act
            var result = await _sut.GetCategoryAsync(categoryVM.Id);

            //Arrange
            _imageConverterService.Verify(x => x.GetImageUrlFromByteArray(It.IsAny<byte[]>()),Times.Once);
            Assert.Equal(category.Id, result.Id);
            Assert.Equal(category.Name, result.Name);
            Assert.Equal(category.Description, result.Description);
        }

        [Fact]
        public async Task GetAllCategoriesAsync_GetListOfCategoriesAndCheckAreEqualLikeModels()
        {
            //Arrange
            var category1 = new Category() { Id = 10, Name = "GoodCategory", Description = "test" };
            var category2 = new Category() { Id = 11, Name = "BadCategory", Description = "xtest" };
            List<Category> categories = new() { category1, category2 };
            var categoryVM1 = new CategoryVM() { Id = 10, Name = "GoodCategory", Description = "test" };
            var categoryVM2 = new CategoryVM() { Id = 11, Name = "BadCategory", Description = "xtest" };
            List<CategoryVM> categoriesVM = new() { categoryVM1, categoryVM2 };

            _categoryRepository.Setup(x => x.GetAllCategoriesAsync()).ReturnsAsync(categories.AsQueryable);
            _mapper.Setup(x => x.Map<List<CategoryVM>>(categories)).Returns(categoriesVM);
            //Act
            var results = await _sut.GetAllCategoriesAsync();

            //Assert
            for (int i = 0; i < results.Count; i++)
            {
                Assert.Equal(categories[i].Id, results[i].Id);
                Assert.Equal(categories[i].Name, results[i].Name);
                Assert.Equal(categories[i].Description, results[i].Description);
            }
        }

        [Fact]
        public async Task UpdateCategoryAsync_ShouldRunsUpdateOnce()
        {
            //Arrange
            var category = new Category() { Id = 10, Name = "GoodCategory", Description = "test", Image = new byte[]{1,2,3} };
            var categoryVM = new CategoryVM() { Id = 10, Name = "BadCategory", Description = "xtest" };

            _mapper.Setup(x => x.Map<Category>(categoryVM)).Returns(category);
            _imageConverterService.Setup(x => x.GetByteArrayFromImageAsync(_fileMock.Object)).ReturnsAsync(category.Image);
            //Act
            await _sut.UpdateCategoryAsync(categoryVM);
            //Assert
            _categoryRepository.Verify(x => x.UpdateCategoryAsync(It.IsAny<Category>()), Times.Once);
            _imageConverterService.Verify(x => x.GetByteArrayFromImageAsync(It.IsAny<IFormFile>()), Times.Once);
            Assert.Equal(category.Id, categoryVM.Id);
        }

        [Fact]
        public async Task DeleteCategoryAsync_ShouldDeleteMethodRunsOnce()
        {
            //Arrange
            var category = new Category() { Id = 10 };

            //Act
            await _sut.DeleteCategoryAsync(category.Id);

            //Assert
            _categoryRepository.Verify(x => x.DeleteCategoryAsync(It.IsAny<int>()), Times.Once);
        }
    }
}
