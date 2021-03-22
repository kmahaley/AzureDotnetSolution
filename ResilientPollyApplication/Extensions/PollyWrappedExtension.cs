using Microsoft.Extensions.DependencyInjection;
using ResilientPollyApplication.Services;

namespace ResilientPollyApplication.Extensions
{
    public static class PollyWrappedExtension
    {
        public static IServiceCollection AddPollyWrappedBasedDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IHttpService, PollyWrappedService>();

            return services;
        }
    }
}