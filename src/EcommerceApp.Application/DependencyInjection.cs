using System.Reflection;
using EcommerceApp.Application.Filters;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.Policies.CanAccessEmployeePanel;
using EcommerceApp.Application.Resources;
using EcommerceApp.Application.Services;
using EcommerceApp.Application.Validations;
using EcommerceApp.Application.ViewModels.AdminPanel;
using EcommerceApp.Application.ViewModels.EmployeePanel;
using EcommerceApp.Application.ViewModels.Order;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IApiLoginService, ApiLoginService>();
            services.AddScoped(typeof(IPaginationService<>), typeof(PaginationService<>));
            services.AddScoped<CheckCheckoutGetPermission>();
            services.AddScoped<CheckCheckoutPostPermission>();
            services.AddMvc().AddFluentValidation();
            services.AddTransient<IValidator<EmployeeVM>, EmployeeVMValidator>();
            services.AddTransient<IValidator<CategoryVM>, CategoryVMValidator>();
            services.AddTransient<IValidator<ProductVM>, ProductVMValidator>();
            services.AddTransient<IValidator<IFormFile>, FileValidator>();
            services.AddTransient<IValidator<OrderCheckoutVM>, OrderCheckoutVMValidator>();
            services.AddSingleton<IAuthorizationHandler, HasAdminClaimHandler>();
            services.AddSingleton<IAuthorizationHandler, HasIsEmployeeClaimHandler>();
            services.AddSingleton<ISearchSelectList, SearchSelectList>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("EmployeePanelEntry", policyBuilder => policyBuilder.Requirements.Add(new CanAccessEmployeePanelRequirement()));
            });

            return services;
        }
    }
}