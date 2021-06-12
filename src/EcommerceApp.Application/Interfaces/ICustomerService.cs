using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceApp.Application.ViewModels.AdminPanel;

namespace EcommerceApp.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<ListCustomerVM> GetAllCustomersAsync();
        Task<CustomerVM> GetCustomerAsync(int customerId);
        Task DeleteCustomerAsync(int customerId);
    }
}
