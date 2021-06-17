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

        public async Task<ListCategoryForListVM> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllCategories().ToListAsync();
            var categoriesVM = _mapper.Map<List<CategoryForListVM>>(categories);
            return new ListCategoryForListVM()
            {
                Categories = categoriesVM
            };
        }

        public async Task<ListCategoryForListVM> GetAllPaginatedCategoriesAsync(int pageSize, int pageNumber)
        {
            var categories = await _categoryRepository.GetAllCategories().ToListAsync();
            var categoriesVM = _mapper.Map<List<CategoryForListVM>>(categories);
            var paginatedVM = await _paginationService.CreateAsync(categoriesVM.AsQueryable(), pageNumber, pageSize);
            return new ListCategoryForListVM()
            {
                Categories = paginatedVM.Items,
                TotalPages = paginatedVM.TotalPages,
                CurrentPage = paginatedVM.CurrentPage
            };
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
