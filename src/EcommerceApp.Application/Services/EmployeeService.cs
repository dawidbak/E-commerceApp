using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;

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
        public async Task AddEmployeeAsync(EmployeeVM employee)
        {
            var employeeVM = _mapper.Map<Employee>(employee);
            await _employeeRepository.AddEmployeeAsync(employeeVM);
        }
        public async Task<IQueryable<EmployeeVM>> GetAllEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllEmployeesAsync();
            return _mapper.Map<IQueryable<EmployeeVM>>(employees);
        }
        public async Task<EmployeeVM> GetEmployeeAsync(int id)
        {
            var employee = await _employeeRepository.GetEmployeeAsync(id);
            return _mapper.Map<EmployeeVM>(employee);
        }
        public async Task UpdateEmployeeAsync(EmployeeVM employee)
        {
            var employeeVM = _mapper.Map<Employee>(employee);
            await _employeeRepository.UpdateEmployeeAsync(employeeVM);
        }
        public async Task DeleteEmployeeAsync(int id)
        {
            await _employeeRepository.DeleteEmployeeAsync(id);
        }

    }
}
