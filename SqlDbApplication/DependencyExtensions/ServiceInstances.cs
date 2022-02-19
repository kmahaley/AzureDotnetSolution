using Microsoft.Extensions.DependencyInjection;
using SqlDbApplication.Services;

namespace SqlDbApplication.DependencyExtensions
{
    public static class ServiceInstances
    {
        public static IServiceCollection AddServiceInstances(this IServiceCollection services)
        {
            //Long runnning background service
            services.AddHostedService<BackgroundDatabaseService>();

            //Business services
            services.AddSingleton<IProductService, ProductService>();
            services.AddSingleton<IFireAndForgetService, FireAndForgetService>();

            return services;
        }
    }
}

