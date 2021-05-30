using System.Reflection;
using EcommerceApp.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using EcommerceApp.Application.Services;
using EcommerceApp.Application.ViewModels;
using FluentValidation.AspNetCore;
using FluentValidation;

namespace EcommerceApp.Application
{
    public static class DependencyIncjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<IEmployeeService,EmployeeService>();
            services.AddMvc().AddFluentValidation();
            services.AddTransient<IValidator<EmployeeVM>, EmployeeValidator>();
            
            return services;
        }
    }
}