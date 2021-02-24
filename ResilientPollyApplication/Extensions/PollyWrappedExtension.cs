using Microsoft.Extensions.DependencyInjection;
using ResilientPollyApplication.Handlers;
using ResilientPollyApplication.Polly;
using ResilientPollyApplication.Services;

namespace ResilientPollyApplication.Extensions
{
    public static class PollyWrappedExtension
    {
        public static IServiceCollection AddPollyWrappedBasedDependencies(this IServiceCollection services)
        {
            services
                .AddHttpClient("PollyWrappedService")
                .AddHttpMessageHandler<TimingHttpMessageHandler>();

            services.AddSingleton<IHttpService, PollyWrappedService>();

            return services;
        }
    }
}
