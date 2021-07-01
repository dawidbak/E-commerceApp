using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceApp.Domain.Models;
using EcommerceApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EcommerceApp.Infrastructure.Tests.Repositories.UnitTests
{
    public class CategoryRepositoryUnitTests
    {
        private readonly DbContextOptions<AppDbContext> _options;

        public CategoryRepositoryUnitTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        }

        [Fact]
        public async Task AddCategoryAsync_CheckCategoryExistsAfterAdd()
        {
            //Arrange
            var category = new Category() { Id = 10, Name = "GoodCategory", Description = "test" };

            using (var context = new AppDbContext(_options))
            {
                //Act
                await context.Database.EnsureCreatedAsync();
                var sut = new CategoryRepository(context);
                await sut.AddCategoryAsync(category);
                var categoryResult = await context.Categories.FindAsync(category.Id);

                //Assert
                Assert.Equal(category, categoryResult);
            }
        }

        [Fact]
        public async Task GetCategoryAsync_GetCategoryAndCheckIfEqualToModel()
        {
            //Arrange
            var category = new Category() { Id = 10, Name = "GoodCategory", Description = "test" };

            using (var context = new AppDbContext(_options))
            {
                //Act
                await context.Database.EnsureCreatedAsync();
                await context.AddAsync(category);
                await context.SaveChangesAsync();
                var sut = new CategoryRepository(context);
                var getCategory = await sut.GetCategoryAsync(category.Id);

                //Assert
                Assert.Equal(category.Id, getCategory.Id);
                Assert.Equal(category.Name, getCategory.Name);
                Assert.Equal(category.Description, getCategory.Description);
            }
        }

        [Fact]
        public async Task GetAllCategories_GetListOfCategoriesAndCheckAreEqualLikeModels()
        {
            //Arrange
            var category1 = new Category() { Id = 10, Name = "GoodCategory", Description = "test" };
            var category2 = new Category() { Id = 11, Name = "BadCategory", Description = "testx" };
            List<Category> categories = new() { category1, category2 };

            using (var context = new AppDbContext(_options))
            {
                //Act
                await context.Database.EnsureCreatedAsync();
                await context.AddRangeAsync(categories);
                await context.SaveChangesAsync();
                var sut = new CategoryRepository(context);
                var getCategories = await sut.GetAllCategories().ToListAsync();

                //Assert
                Assert.Equal(categories, getCategories);
            }
        }

        [Fact]
        public async Task UpdateCategoryAsync_ShouldUpdateCategoryNameAndDescription()
        {
            //Arrange
            var category = new Category() { Id = 10, Name = "GoodCategory", Description = "test" };
            var updatedCategory = new Category() { Id = 10, Name = "BadCategory", Description = "testx", Image = new byte[]{2,3,4} };

            using (var context = new AppDbContext(_options))
            {
                //Act
                await context.Database.EnsureCreatedAsync();
                await context.AddAsync(category);
                await context.SaveChangesAsync();
            }
            using (var context = new AppDbContext(_options))
            {
                //Act
                await context.Database.EnsureCreatedAsync();
                var sut = new CategoryRepository(context);
                await sut.UpdateCategoryAsync(updatedCategory);
                await context.SaveChangesAsync();
                var categoryAfterUpdate = await context.Categories.FindAsync(category.Id);

                //Assert
                Assert.Equal(updatedCategory.Id, categoryAfterUpdate.Id);
                Assert.Equal(updatedCategory.Name, categoryAfterUpdate.Name);
                Assert.Equal(updatedCategory.Description, categoryAfterUpdate.Description);
            }
        }

        [Fact]
        public async Task DeleteCategoryAsync_CategoryShouldNotExistsAfterDelete()
        {
            //Assert
            var category1 = new Category() { Id = 10, Name = "GoodCategory", Description = "test" };
            var category2 = new Category() { Id = 11, Name = "BadCategory", Description = "testx" };

            using (var context = new AppDbContext(_options))
            {
                //Act
                await context.Database.EnsureCreatedAsync();
                var sut = new CategoryRepository(context);
                await context.AddAsync(category1);
                await context.AddAsync(category2);
                await context.SaveChangesAsync();
                await sut.DeleteCategoryAsync(category1.Id);
                var getCategory1 = await context.Categories.FindAsync(category1.Id);
                var getCategory2 = await context.Categories.FindAsync(category2.Id);

                //Assert
                Assert.Null(getCategory1);
                Assert.Equal(category2, getCategory2);
            }
        }
    }
}