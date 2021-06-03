using System.Reflection;
using EcommerceApp.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using EcommerceApp.Application.Services;
using EcommerceApp.Application.ViewModels.AdminPanel;
using FluentValidation.AspNetCore;
using FluentValidation;
using EcommerceApp.Application.ViewModels.EmployeePanel;

namespace EcommerceApp.Application
{
    public static class DependencyIncjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<IEmployeeService,EmployeeService>();
            services.AddScoped<ICategoryService,CategoryService>();
            services.AddMvc().AddFluentValidation();
            services.AddTransient<IValidator<EmployeeVM>, EmployeeValidator>();
            services.AddTransient<IValidator<CategoryVM>, CategoryValidator>();
            
            return services;
        }
    }
}