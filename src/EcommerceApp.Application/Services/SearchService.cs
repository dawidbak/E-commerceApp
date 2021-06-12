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
        public SearchService(ICategoryService categoryService, IProductService productService, IEmployeeService employeeService, ICustomerService customerService)
        {
            _categoryService = categoryService;
            _productService = productService;
            _employeeService = employeeService;
            _customerService = customerService;
        }
        public async Task<ListCategoryForListVM> SearchSelectedCategoryAsync(string selectedValue, string searchString)
        {
            ListCategoryForListVM model = new();
            var intParse = int.TryParse(searchString, out int id);

            model.Categories =  selectedValue switch
            {
                "Id" => intParse ? (await _categoryService.GetAllCategoriesAsync()).Categories.Where(x => x.Id == id).ToList() : model.Categories,
                "Name" => (await _categoryService.GetAllCategoriesAsync()).Categories.Where(x => x.Name.Contains(searchString)).ToList(),
                _ => model.Categories,
            };
            return model;
        }

        public async Task<ListProductForListVM> SearchSelectedProductAsync(string selectedValue, string searchString)
        {
            ListProductForListVM model = new();
            var idParse = int.TryParse(searchString, out int id);
            var unitPriceParse = decimal.TryParse(searchString, out decimal price);
            var unitsInStock = int.TryParse(searchString, out int units);

            model.Products = selectedValue switch
            {
                "Id" => idParse ? (await _productService.GetAllProductsAsync()).Products.Where(x => x.Id == id).ToList() : model.Products,
                "Name" => (await _productService.GetAllProductsAsync()).Products.Where(x => x.CategoryName.Contains(searchString)).ToList(),
                "UnitPrice" => unitPriceParse ? (await _productService.GetAllProductsAsync()).Products.Where(x => x.UnitPrice == price).ToList() : model.Products,
                "UnitsInStock" => unitsInStock ? (await _productService.GetAllProductsAsync()).Products.Where(x => x.UnitsInStock == units).ToList() : model.Products,
                "CategoryName" => (await _productService.GetAllProductsAsync()).Products.Where(x => x.Name.Contains(searchString)).ToList(),
                _ => model.Products,
            };
            return model;
        }

        public async Task<ListEmployeeForListVM> SearchSelectedEmployeeAsync(string selectedValue, string searchString)
        {
            ListEmployeeForListVM model = new();
            var idParse = int.TryParse(searchString, out int id);

            model.Employees = selectedValue switch
            {
                "Id" => idParse ? (await _employeeService.GetAllEmployeesAsync()).Employees.Where(x => x.Id == id).ToList() : model.Employees,
                "FirstName" => (await _employeeService.GetAllEmployeesAsync()).Employees.Where(x => x.FirstName.Contains(searchString)).ToList(),
                "Email" => (await _employeeService.GetAllEmployeesAsync()).Employees.Where(x => x.Email.Contains(searchString)).ToList(),
                "LastName" => (await _employeeService.GetAllEmployeesAsync()).Employees.Where(x => x.LastName.Contains(searchString)).ToList(),
                "Position" => (await _employeeService.GetAllEmployeesAsync()).Employees.Where(x => x.Position.Contains(searchString)).ToList(),
                _ => model.Employees,
            };
            return model;
        }

        public async Task<ListCustomerVM> SearchSelectedCustomerAsync(string selectedValue, string searchString)
        {
            ListCustomerVM model = new();
            var idParse = int.TryParse(searchString, out int id);

            model.Customers =  selectedValue switch
            {
                "Id" => idParse ? (await _customerService.GetAllCustomersAsync()).Customers.Where(x => x.Id == id).ToList() : model.Customers,
                "FirstName" => (await _customerService.GetAllCustomersAsync()).Customers.Where(x => x.FirstName.Contains(searchString)).ToList(),
                "Email" => (await _customerService.GetAllCustomersAsync()).Customers.Where(x => x.Email.Contains(searchString)).ToList(),
                "LastName" => (await _customerService.GetAllCustomersAsync()).Customers.Where(x => x.LastName.Contains(searchString)).ToList(),
                "City" => (await _customerService.GetAllCustomersAsync()).Customers.Where(x => x.City.Contains(searchString)).ToList(),
                "PostalCode" => (await _customerService.GetAllCustomersAsync()).Customers.Where(x => x.PostalCode.Contains(searchString)).ToList(),
                "Address" => (await _customerService.GetAllCustomersAsync()).Customers.Where(x => x.Address.Contains(searchString)).ToList(),
                "PhoneNumber" => (await _customerService.GetAllCustomersAsync()).Customers.Where(x => x.PhoneNumber.Contains(searchString)).ToList(),
                _ => model.Customers,
            };
            return model;
        }
    }
}
