using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.EmployeePanel;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository,IMapper mapper)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }
        public async Task AddCategoryAsync(CategoryVM categoryVM)
        {
            var category = _mapper.Map<Category>(categoryVM);
            await _categoryRepository.AddCategoryAsync(category);
        }

        public async Task DeleteCategoryAsync(int categoryVMId)
        {
           await _categoryRepository.DeleteCategoryAsync(categoryVMId);
        }

        public async Task<List<CategoryVM>> GetAllCategoriesAsync()
        {
            var categories =  (await _categoryRepository.GetAllCategoriesAsync()).ToList();
            return _mapper.Map<List<CategoryVM>>(categories);
        }

        public async Task<CategoryVM> GetCategoryAsync(int categoryVMId)
        {
            var category = await _categoryRepository.GetCategoryAsync(categoryVMId);
            return _mapper.Map<CategoryVM>(category);
        }

        public async Task UpdateCategoryAsync(CategoryVM categoryVM)
        {
            var category = _mapper.Map<Category>(categoryVM);
            await _categoryRepository.UpdateCategoryAsync(category);
        }
    }
}
