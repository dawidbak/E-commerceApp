using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using EcommerceApp.Domain.Models;
using EcommerceApp.Domain.Interfaces;
using Microsoft.EntityFrameworkCore.InMemory;

namespace EcommerceApp.Infrastructure.Repositories.UnitTests
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
            var category = new Category(){CategoryId = 10, Name ="GoodCategory", Description = "test"};

            using (var context = new AppDbContext(_options))
            {
                //Act
                context.Database.EnsureCreated();
                var sut = new CategoryRepository(context);
                await sut.AddCategoryAsync(category);
                var categoryResult = await context.Categories.FindAsync(category.CategoryId);

                //Assert
                Assert.Equal(category, categoryResult);
            }
        }

        [Fact]
        public async Task GetCategoryAsync_GetCategoryAndCheckIfEqualToModel()
        {
            //Arrange
            var category = new Category(){CategoryId = 10, Name ="GoodCategory", Description = "test"};

            using (var context = new AppDbContext(_options))
            {
                //Act
                context.Database.EnsureCreated();
                context.Add(category);
                context.SaveChanges();
                var sut = new CategoryRepository(context);
                var getCategory = await sut.GetCategoryAsync(category.CategoryId);

                //Assert
                Assert.Equal(category.CategoryId, getCategory.CategoryId);
                Assert.Equal(category.Name, getCategory.Name);
                Assert.Equal(category.Description, getCategory.Description);
            }
        }

        [Fact]
        public async Task GetAllCategoriesAsync_GetListOfCategoriesAndCheckAreEqualLikeModels()
        {
            //Arrange
            var category1 = new Category(){CategoryId = 10, Name ="GoodCategory", Description = "test"};
            var category2 = new Category(){CategoryId = 11, Name ="BadCategory", Description = "testx"};
            List<Category> categories = new() { category1, category2 };

            using (var context = new AppDbContext(_options))
            {
                //Act
                context.Database.EnsureCreated();
                await context.AddRangeAsync(categories);
                await context.SaveChangesAsync();
                var sut = new CategoryRepository(context);
                var getCategories = await sut.GetAllCategoriesAsync();

                //Assert
                Assert.Equal(categories, getCategories);
            }
        }

        [Fact]
        public async Task UpdateCategoryAsync_ShouldUpdateCategoryNameAndDescription()
        {
            //Arrange
            var category = new Category(){CategoryId = 10, Name ="GoodCategory", Description = "test"};
            var updatedCategory = new Category(){CategoryId = 10, Name ="BadCategory", Description = "testx"};

            using (var context = new AppDbContext(_options))
            {
                //Act
                context.Database.EnsureCreated();
                await context.AddAsync(category);
                await context.SaveChangesAsync();
            }
            using (var context = new AppDbContext(_options))
            {
                //Act
                context.Database.EnsureCreated();
                var sut = new CategoryRepository(context);
                await sut.UpdateCategoryAsync(updatedCategory);
                var categoryAfterUpdate = await context.Categories.FindAsync(category.CategoryId);

                //Assert
                Assert.Equal(updatedCategory.CategoryId, categoryAfterUpdate.CategoryId);
                Assert.Equal(updatedCategory.Name, categoryAfterUpdate.Name);
                Assert.Equal(updatedCategory.Description, categoryAfterUpdate.Description);
            }

        }

        [Fact]
        public async Task DeleteCategoryAsync_CheckCategoryExistsAfterDelete()
        {
            //Assert
            var category1 = new Category(){CategoryId = 10, Name ="GoodCategory", Description = "test"};
            var category2 = new Category(){CategoryId = 11, Name ="BadCategory", Description = "testx"};

            using (var context = new AppDbContext(_options))
            {
                //Act
                context.Database.EnsureCreated();
                var sut = new CategoryRepository(context);
                await context.AddAsync(category1);
                await context.AddAsync(category2);
                await context.SaveChangesAsync();
                await sut.DeleteCategoryAsync(category1.CategoryId);
                var getCategory1 = await context.Categories.FindAsync(category1.CategoryId);
                var getCategory2 = await context.Categories.FindAsync(category2.CategoryId);

                //Assert
                Assert.Null(getCategory1);
                Assert.Equal(category2, getCategory2);
            }
        }
    }
}