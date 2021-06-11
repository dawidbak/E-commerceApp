using System.Reflection;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.Policies.CanAccessEmployeePanel;
using EcommerceApp.Application.Services;
using EcommerceApp.Application.ViewModels.AdminPanel;
using EcommerceApp.Application.ViewModels.EmployeePanel;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using EcommerceApp.Application.Validations;
using Microsoft.AspNetCore.Http;
using EcommerceApp.Application.Resources;

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
            services.AddScoped<IImageConverterService, ImageConverterService>();
            services.AddScoped<ISearchService, SearchService>();
            services.AddScoped<IHomeService, HomeService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddMvc().AddFluentValidation();
            services.AddTransient<IValidator<EmployeeVM>, EmployeeVMValidator>();
            services.AddTransient<IValidator<CategoryVM>, CategoryVMValidator>();
            services.AddTransient<IValidator<ProductVM>, ProductVMValidator>();
            services.AddTransient<IValidator<IFormFile>,FileValidator>();
            services.AddSingleton<IAuthorizationHandler, HasAdminClaimHandler>();
            services.AddSingleton<IAuthorizationHandler, HasIsEmployeeClaimHandler>();
            services.AddSingleton<ISearchSelectList,SearchSelectList>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("EmployeePanelEntry", policyBuilder => policyBuilder.Requirements.Add(new CanAccessEmployeePanelRequirement()));
            });

            return services;
        }
    }
}