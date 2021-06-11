using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.AdminPanel;
using EcommerceApp.Infrastructure.Repositories;
using EcommerceApp.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EcommerceApp.Web.Controllers
{
    [Authorize("Admin")]
    public class AdminPanelController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly ISearchService _searchService;
        private readonly ICustomerService _customerService;
        private readonly ILogger<AdminPanelController> _logger;
        public AdminPanelController(IEmployeeService employeeService, ILogger<AdminPanelController> logger, ISearchService searchService, ICustomerService customerService)
        {
            _employeeService = employeeService;
            _logger = logger;
            _searchService = searchService;
            _customerService = customerService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Employees(string selectedValue, string searchString)
        {
            if (!string.IsNullOrEmpty(selectedValue) && !string.IsNullOrEmpty(searchString))
            {
                return View(await _searchService.SearchSelectedEmployeeAsync(selectedValue, searchString));
            }
            return View(await _employeeService.GetAllEmployeesAsync());
        }

        public async Task<IActionResult> Customers(string selectedValue, string searchString)
        {
            if (!string.IsNullOrEmpty(selectedValue) && !string.IsNullOrEmpty(searchString))
            {
                return View(await _searchService.SearchSelectedCustomerAsync(selectedValue, searchString));
            }
            return View(await _customerService.GetAllCustomersAsync());
        }

        [HttpGet]
        public IActionResult AddEmployee()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(EmployeeVM employeeVM)
        {
            if (ModelState.IsValid)
            {
                await _employeeService.AddEmployeeAsync(employeeVM);
                return RedirectToAction("Index");
            }
            return BadRequest();
        }

        public async Task<IActionResult> DeleteEmployee(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound("You must pass a valid Employee ID in the route, for example, /AdminPanel/DeleteEmployee/21");
            }
            await _employeeService.DeleteEmployeeAsync(id.Value);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteCustomer(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound("You must pass a valid Customer ID in the route, for example, /AdminPanel/DeleteCustomer/21");
            }
            await _customerService.DeleteCustomerAsync(id.Value);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> EditEmployee(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound("You must pass a valid Employee ID in the route, for example, /AdminPanel/EditEmployee/21");
            }
            var model = await _employeeService.GetEmployeeAsync(id.Value);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditEmployee(EmployeeVM employeeVM)
        {
            if (ModelState.IsValid)
            {
                await _employeeService.UpdateEmployeeAsync(employeeVM);
                return RedirectToAction("Index");
            }
            return BadRequest();
        }
    }
}