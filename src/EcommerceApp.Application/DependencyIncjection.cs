using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace EcommerceApp.Application
{
    public static class DependencyIncjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            
            return services;
        }
    }
}