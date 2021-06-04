using System.Reflection;
using EcommerceApp.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using EcommerceApp.Application.Services;
using EcommerceApp.Application.ViewModels.AdminPanel;
using FluentValidation.AspNetCore;
using FluentValidation;
using EcommerceApp.Application.ViewModels.EmployeePanel;
using EcommerceApp.Application.Policies.CanAccessEmployeePanel;
using Microsoft.AspNetCore.Authorization;

namespace EcommerceApp.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddMvc().AddFluentValidation();
            services.AddTransient<IValidator<EmployeeVM>, EmployeeValidator>();
            services.AddTransient<IValidator<CategoryVM>, CategoryValidator>();
            services.AddTransient<IValidator<ProductVM>, ProductValidator>();
            services.AddSingleton<IAuthorizationHandler, HasAdminClaimHandler>();
            services.AddSingleton<IAuthorizationHandler, HasIsEmployeeClaimHandler>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("EmployeePanelEntry", policyBuilder => policyBuilder.Requirements.Add(new CanAccessEmployeePanelRequirement()));
            });

            return services;
        }
    }
}