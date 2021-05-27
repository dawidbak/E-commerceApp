using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using AutoMapper.QueryableExtensions;
using System.Collections.Generic;

namespace EcommerceApp.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        public EmployeeService(IMapper mapper, IEmployeeRepository employeeRepository)
        {
            _mapper = mapper;
            _employeeRepository = employeeRepository;
        }
        public async Task AddEmployeeAsync(EmployeeVM employeeVM)
        {
            var employee = _mapper.Map<Employee>(employeeVM);
            await _employeeRepository.AddEmployeeAsync(employee);
        }
        public async Task<List<EmployeeVM>> GetAllEmployeesAsync()
        {
            var employees = (await _employeeRepository.GetAllEmployeesAsync()).ToList();
            return _mapper.Map<List<EmployeeVM>>(employees);
            
        }
        public async Task<EmployeeVM> GetEmployeeAsync(int id)
        {
            var employee = await _employeeRepository.GetEmployeeAsync(id);
            return _mapper.Map<EmployeeVM>(employee);
        }
        public async Task UpdateEmployeeAsync(EmployeeVM employeeVM)
        {
            var employee = _mapper.Map<Employee>(employeeVM);
            await _employeeRepository.UpdateEmployeeAsync(employee);
        }
        public async Task DeleteEmployeeAsync(int id)
        {
            await _employeeRepository.DeleteEmployeeAsync(id);
        }

    }
}
