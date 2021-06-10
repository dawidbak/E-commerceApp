using System.Reflection;
using EcommerceApp.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using EcommerceApp.Domain.Interfaces;

namespace EcommerceApp.Infrastructure
{
    public static class DependencyIncjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IEmployeeRepository,EmployeeRepository>();
            services.AddScoped<ICategoryRepository,CategoryRepository>();
            services.AddScoped<IProductRepository,ProductRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICartItemRepository,CartItemRepository>();
            
            return services;
        }
    }
}