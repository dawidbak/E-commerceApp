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
        private readonly IImageConverterService _imageConverterService;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper, IImageConverterService imageConverterService)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _imageConverterService = imageConverterService;
        }
        public async Task AddCategoryAsync(CategoryVM categoryVM)
        {
            var category = _mapper.Map<Category>(categoryVM);
            category.Image = await _imageConverterService.GetByteArrayFromImageAsync(categoryVM.ImageFormFile);
            await _categoryRepository.AddCategoryAsync(category);
        }

        public async Task DeleteCategoryAsync(int categoryVMId)
        {
            await _categoryRepository.DeleteCategoryAsync(categoryVMId);
        }

        public async Task<List<CategoryVM>> GetAllCategoriesAsync()
        {
            var categories = (await _categoryRepository.GetAllCategoriesAsync()).ToList();
            return _mapper.Map<List<CategoryVM>>(categories);
        }

        public async Task<CategoryVM> GetCategoryAsync(int categoryVMId)
        {
            var category = await _categoryRepository.GetCategoryAsync(categoryVMId);
            var categoryVM = _mapper.Map<CategoryVM>(category);
            categoryVM.ImageUrl = _imageConverterService.GetImageUrlFromByteArray(category.Image);
            return categoryVM;
        }

        public async Task UpdateCategoryAsync(CategoryVM categoryVM)
        {
            var category = _mapper.Map<Category>(categoryVM);
            category.Image = await _imageConverterService.GetByteArrayFromImageAsync(categoryVM.ImageFormFile);
            await _categoryRepository.UpdateCategoryAsync(category);
        }
    }
}
