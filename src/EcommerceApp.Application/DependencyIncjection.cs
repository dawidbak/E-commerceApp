using System.Reflection;
using EcommerceApp.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using EcommerceApp.Application.Services;

namespace EcommerceApp.Application
{
    public static class DependencyIncjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<IEmployeeService,EmployeeService>();
            
            return services;
        }
    }
}