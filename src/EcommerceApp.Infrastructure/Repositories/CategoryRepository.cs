using System;
using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _appDbContext;

        public CategoryRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task AddCategoryAsync(Category category)
        {
            await _appDbContext.Categories.AddAsync(category);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            var category = await _appDbContext.Categories.FindAsync(categoryId);
            if (category != null)
            {
                _appDbContext.Categories.Remove(category);
                await _appDbContext.SaveChangesAsync();
            }
        }

        public IQueryable<Category> GetAllCategories()
        {
            return _appDbContext.Categories.AsQueryable();
        }

        public async Task<Category> GetCategoryAsync(int categoryId)
        {
            return await _appDbContext.Categories.FindAsync(categoryId);
        }

        public async Task<Category> GetCategoryAsync(string categoryName)
        {
            return await _appDbContext.Categories.FirstOrDefaultAsync(x => x.Name == categoryName);
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            _appDbContext.Categories.Update(category);
            if(category.Image.Length == 0)
            {
                _appDbContext.Entry<Category>(category).Property(x => x.Image).IsModified = false;
            }
            await _appDbContext.SaveChangesAsync();
        }
    }
}
