using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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
        private readonly IPaginationService<CategoryForListVM> _paginationService;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper, IImageConverterService imageConverterService, IPaginationService<CategoryForListVM> paginationService)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _imageConverterService = imageConverterService;
            _paginationService = paginationService;
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

        public async Task<ListCategoryForListVM> GetAllPaginatedCategoriesAsync(int pageSize, int pageNumber)
        {
            var categories = _categoryRepository.GetAllCategories().ProjectTo<CategoryForListVM>(_mapper.ConfigurationProvider);
            var paginatedVM = await _paginationService.CreateAsync(categories, pageNumber, pageSize);
            return _mapper.Map<ListCategoryForListVM>(paginatedVM);
        }
        public async Task<List<string>> GetAllCategoriesNamesAsync()
        {
            return await _categoryRepository.GetAllCategories().Select(x => x.Name).ToListAsync();
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
