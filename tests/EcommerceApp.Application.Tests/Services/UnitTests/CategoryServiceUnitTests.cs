using System.Security.AccessControl;
using System.Reflection.PortableExecutable;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.Mapping;
using EcommerceApp.Application.Services;
using EcommerceApp.Application.ViewModels;
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
        private readonly Mock<IMapper> _mapper = new();
        private readonly Mock<ICategoryRepository> _categoryRepository = new();
        private readonly Mock<IImageConverterService> _imageConverterService = new();
        private readonly Mock<IFormFile> _fileMock = new();
        private readonly Mock<IPaginationService<CategoryForListVM>> _paginationService = new();

        public CategoryServiceUnitTests()
        {
            _sut = new CategoryService(_categoryRepository.Object, _mapper.Object, _imageConverterService.Object, _paginationService.Object);
        }

        [Fact]
        public async Task AddCategoryAsync_ShouldCategoryAddMethodRunsOnce()
        {
            //Arrange
            var category = new Category() { Id = 10, Name = "GoodCategory", Description = "test", Image = new byte[] { 1, 2, 3 } };
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
            var category = new Category() { Id = 10, Name = "GoodCategory", Description = "test", Image = new byte[] { 1, 2, 3 } };
            var categoryVM = new CategoryVM() { Id = 10, Name = "GoodCategory", Description = "test", ImageUrl = "test123" };

            _categoryRepository.Setup(x => x.GetCategoryAsync(category.Id)).ReturnsAsync(category);
            _mapper.Setup(x => x.Map<CategoryVM>(category)).Returns(categoryVM);
            _imageConverterService.Setup(x => x.GetImageUrlFromByteArray(category.Image)).Returns(categoryVM.ImageUrl);

            //Act
            var result = await _sut.GetCategoryAsync(categoryVM.Id);

            //Arrange
            _imageConverterService.Verify(x => x.GetImageUrlFromByteArray(It.IsAny<byte[]>()), Times.Once);
            Assert.Equal(category.Id, result.Id);
            Assert.Equal(category.Name, result.Name);
            Assert.Equal(category.Description, result.Description);
        }

        [Fact]
        public async Task GetAllPaginatedCategoriesAsync_ReturnsListCategoryForListVMAndCheckAreEqualLikeModel()
        {
            //Arrange
            var category1 = new Category() { Id = 10, Name = "GoodCategory", Description = "test" };
            var category2 = new Category() { Id = 11, Name = "BadCategory", Description = "xtest" };
            List<Category> categories = new() { category1, category2 };
            var categoryVM1 = new CategoryForListVM() { Id = 10, Name = "GoodCategory" };
            var categoryVM2 = new CategoryForListVM() { Id = 11, Name = "BadCategory" };
            List<CategoryForListVM> categoryForListVMs = new() { categoryVM1, categoryVM2 };

            PaginatedVM<CategoryForListVM> paginatedVM = new()
            {
                Items = categoryForListVMs,
                CurrentPage = 1,
                TotalPages = 2,
            };
            ListCategoryForListVM listCategoryForListVM = new()
            {
                Categories = categoryForListVMs,
                CurrentPage = 1,
                TotalPages = 2,
            };
            _mapper.Setup(x => x.ConfigurationProvider).Returns(() => new MapperConfiguration(cfg =>
            { cfg.CreateMap<Category, CategoryForListVM>(); }));
            _categoryRepository.Setup(x => x.GetAllCategories()).Returns(categories.AsQueryable());
            _paginationService.Setup(x => x.CreateAsync(It.IsAny<IQueryable<CategoryForListVM>>(), 1,10)).ReturnsAsync(paginatedVM);
            _mapper.Setup(x => x.Map<ListCategoryForListVM>(paginatedVM)).Returns(listCategoryForListVM);

            //Act
            var result = await _sut.GetAllPaginatedCategoriesAsync(10, 1);

            //Assert
            Assert.Equal(listCategoryForListVM, result);
        }

        [Fact]
        public async Task UpdateCategoryAsync_ShouldRunsUpdateOnce()
        {
            //Arrange
            var category = new Category() { Id = 10, Name = "GoodCategory", Description = "test", Image = new byte[] { 1, 2, 3 } };
            var categoryVM = new CategoryVM() { Id = 10, Name = "BadCategory", Description = "xtest" };

            _mapper.Setup(x => x.Map<Category>(categoryVM)).Returns(category);
            _imageConverterService.Setup(x => x.GetByteArrayFromImageAsync(_fileMock.Object)).ReturnsAsync(category.Image);

            //Act
            await _sut.UpdateCategoryAsync(categoryVM);

            //Assert
            _categoryRepository.Verify(x => x.UpdateCategoryAsync(It.IsAny<Category>()), Times.Once);
            _imageConverterService.Verify(x => x.GetByteArrayFromImageAsync(It.IsAny<IFormFile>()), Times.Once);
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
