using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EcommerceApp.Web.Models;
using Microsoft.AspNetCore.Authorization;
using EcommerceApp.Infrastructure.Repositories;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels;

namespace EcommerceApp.Web.Controllers
{
    [Authorize("Admin")]
    public class AdminPanelController : Controller
    {
        private readonly IEmployeeService _employeeService;
        public AdminPanelController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _employeeService.GetAllEmployeesAsync();
            return View(model);
        }
        [HttpGet]
        public IActionResult AddEmployee()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(EmployeeVM employeeVM)
        {
            await _employeeService.AddEmployeeAsync(employeeVM);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteEmployee(int Id)
        {
            await _employeeService.DeleteEmployeeAsync(Id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> EditEmployee(int id)
        {
            var model = await _employeeService.GetEmployeeAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditEmployee(EmployeeVM employeeVM)
        {
            await _employeeService.UpdateEmployeeAsync(employeeVM);
            return RedirectToAction("Index");
        }
        
    }
}