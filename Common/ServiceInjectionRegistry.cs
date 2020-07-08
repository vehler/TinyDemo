using Microsoft.Extensions.DependencyInjection;
using TinyDemo.Services;

namespace TinyDemo.Common
{
    public static class ServiceInjectionRegistry
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {


            services.AddScoped<AuthenticationService>();
            services.AddScoped<UserService>();

            return services;
        }
    }
}
