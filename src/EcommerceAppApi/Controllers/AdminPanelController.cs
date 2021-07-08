using System;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.AdminPanel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EcommerceAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Admin")]
    public class AdminPanelController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ISearchService _searchService;
        private readonly ICustomerService _customerService;
        private readonly IConfiguration _configuration;

        public AdminPanelController(IEmployeeService employeeService, ISearchService searchService, IConfiguration configuration, ICustomerService customerService)
        {
            _employeeService = employeeService;
            _searchService = searchService;
            _configuration = configuration;
            _customerService = customerService;
        }

        [HttpGet("Employees")]
        public async Task<IActionResult> Employees(string selectedValue, string searchString, string pageSize, int? pageNumber)
        {
            if (!pageNumber.HasValue)
            {
                pageNumber = 1;
            }
            if (!int.TryParse(pageSize, out int intPageSize))
            {
                intPageSize = _configuration.GetValue("DefaultPageSize", 10);
            }

            if (!string.IsNullOrEmpty(selectedValue) && !string.IsNullOrEmpty(searchString))
            {
                return Ok(await _searchService.SearchSelectedEmployeesAsync(selectedValue, searchString, intPageSize, pageNumber.Value));
            }
            return Ok(await _employeeService.GetAllPaginatedEmployeesAsync(intPageSize, pageNumber.Value));
        }

        [HttpGet("Customers")]
        public async Task<IActionResult> Customers(string selectedValue, string searchString, string pageSize, int? pageNumber)
        {
            if (!pageNumber.HasValue)
            {
                pageNumber = 1;
            }
            if (!int.TryParse(pageSize, out int intPageSize))
            {
                intPageSize = _configuration.GetValue("DefaultPageSize", 10);
            }

            if (!string.IsNullOrEmpty(selectedValue) && !string.IsNullOrEmpty(searchString))
            {
                return Ok(await _searchService.SearchSelectedCustomersAsync(selectedValue, searchString, intPageSize, pageNumber.Value));
            }
            return Ok(await _customerService.GetAllPaginatedCustomersAsync(intPageSize, pageNumber.Value));
        }

        [HttpPost("AddEmployee")]
        public async Task<IActionResult> AddEmployee([FromBody] EmployeeVM employeeVM)
        {
            await _employeeService.AddEmployeeAsync(employeeVM);
            return Ok();
        }

        [HttpDelete("DeleteEmployee/{id}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] int id)
        {
            await _employeeService.DeleteEmployeeAsync(id);
            return Ok();
        }

        [HttpDelete("DeleteCustomer/{id}")]
        public async Task<IActionResult> DeleteCustomer([FromRoute] int id)
        {
            await _customerService.DeleteCustomerAsync(id);
            return Ok();
        }

        [HttpGet("EditEmployee/{id}")]
        public async Task<IActionResult> EditEmployee([FromRoute] int id)
        {
            return Ok(await _employeeService.GetEmployeeAsync(id));
        }

        [HttpPut("EditEmployee")]
        public async Task<IActionResult> EditEmployee([FromBody] EmployeeVM employeeVM)
        {
            await _employeeService.UpdateEmployeeAsync(employeeVM);
            return Ok();
        }

        [HttpGet("CustomerDetails/{id}")]
        public async Task<IActionResult> CustomerDetails([FromRoute] int id)
        {
            return Ok(await _customerService.GetCustomerDetailsAsync(id));
        }

    }
}
