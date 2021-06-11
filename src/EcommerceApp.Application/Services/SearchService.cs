using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.AdminPanel;
using EcommerceApp.Application.ViewModels.EmployeePanel;
using EcommerceApp.Domain.Interfaces;

namespace EcommerceApp.Application.Services
{
    public class SearchService : ISearchService
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IEmployeeService _employeeService;
        private readonly ICustomerService _customerService;
        public SearchService(ICategoryService categoryService, IProductService productService,IEmployeeService employeeService, ICustomerService customerService)
        {
            _categoryService = categoryService;
            _productService = productService;
            _employeeService = employeeService;
            _customerService = customerService;
        }
        public async Task<List<CategoryVM>> SearchSelectedCategoryAsync(string selectedValue, string searchString)
        {
            List<CategoryVM> model = new();
            var intParse = int.TryParse(searchString, out int id);

            return selectedValue switch
            {
                "Id" => intParse ? (await _categoryService.GetAllCategoriesAsync()).Where(x => x.Id == id).ToList() : model,
                "Name" => (await _categoryService.GetAllCategoriesAsync()).Where(x => x.Name.Contains(searchString)).ToList(),
                _ => model,
            };
        }

        public async Task<List<ProductVM>> SearchSelectedProductAsync(string selectedValue, string searchString)
        {
            List<ProductVM> model = new();
            var idParse = int.TryParse(searchString, out int id);
            var unitPriceParse = decimal.TryParse(searchString,out decimal price);
            var unitsInStock = int.TryParse(searchString, out int units);

            return selectedValue switch
            {
                "Id" => idParse ? (await _productService.GetAllProductsAsync()).Where(x => x.Id == id).ToList() : model,
                "Name" => (await _productService.GetAllProductsAsync()).Where(x => x.CategoryName.Contains(searchString)).ToList(),
                "UnitPrice" => unitPriceParse ? (await _productService.GetAllProductsAsync()).Where(x => x.UnitPrice == price).ToList() : model,
                "UnitsInStock" => unitsInStock ? (await _productService.GetAllProductsAsync()).Where(x => x.UnitsInStock == units).ToList() : model,
                "CategoryName" => (await _productService.GetAllProductsAsync()).Where(x => x.Name.Contains(searchString)).ToList(),
                _ => model,
            };
        }

          public async Task<List<EmployeeVM>> SearchSelectedEmployeeAsync(string selectedValue, string searchString)
        {
            List<EmployeeVM> model = new();
            var idParse = int.TryParse(searchString, out int id);

            return selectedValue switch
            {
                "Id" => idParse ? (await _employeeService.GetAllEmployeesAsync()).Where(x => x.Id == id).ToList() : model,
                "FirstName" => (await _employeeService.GetAllEmployeesAsync()).Where(x => x.FirstName.Contains(searchString)).ToList(),
                "Email" => (await _employeeService.GetAllEmployeesAsync()).Where(x => x.Email.Contains(searchString)).ToList(),
                "LastName" => (await _employeeService.GetAllEmployeesAsync()).Where(x => x.LastName.Contains(searchString)).ToList(),
                "Position" => (await _employeeService.GetAllEmployeesAsync()).Where(x => x.Position.Contains(searchString)).ToList(),
                _ => model,
            };
        }

        public async Task<List<CustomerVM>> SearchSelectedCustomerAsync(string selectedValue, string searchString)
        {
            List<CustomerVM> model = new();
            var idParse = int.TryParse(searchString, out int id);

            return selectedValue switch
            {
                "Id" => idParse ? (await _customerService.GetAllCustomersAsync()).Where(x => x.Id == id).ToList() : model,
                "FirstName" => (await _customerService.GetAllCustomersAsync()).Where(x => x.FirstName.Contains(searchString)).ToList(),
                "Email" => (await _customerService.GetAllCustomersAsync()).Where(x => x.Email.Contains(searchString)).ToList(),
                "LastName" => (await _customerService.GetAllCustomersAsync()).Where(x => x.LastName.Contains(searchString)).ToList(),
                "City" => (await _customerService.GetAllCustomersAsync()).Where(x => x.City.Contains(searchString)).ToList(),
                "PostalCode" => (await _customerService.GetAllCustomersAsync()).Where(x => x.PostalCode.Contains(searchString)).ToList(),
                "Address" => (await _customerService.GetAllCustomersAsync()).Where(x => x.Address.Contains(searchString)).ToList(),
                "PhoneNumber" => (await _customerService.GetAllCustomersAsync()).Where(x => x.PhoneNumber.Contains(searchString)).ToList(),
                _ => model,
            };
        }
    }
}
