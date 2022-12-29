using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlDbApplication.Configurations;
using SqlDbApplication.Services;
using SqlDbApplication.Services.Interface;

namespace SqlDbApplication.DependencyExtensions
{
    public static class ServiceInstances
    {
        public static IServiceCollection AddServiceInstances(this IServiceCollection services, IConfiguration configuration)
        {
            //Long runnning background service
            services.AddHostedService<BackgroundDatabaseService>();

            //Configurations
            services.Configure<AuthenticationConfiguration>(configuration.GetSection("AuthenticationConfiguration"));

            //Business services
            services.AddSingleton<IProductService, ProductService>();
            services.AddSingleton<IFireAndForgetService, FireAndForgetService>();

            services.AddScoped<ICityService, CityService>();
            services.AddScoped<IPointOfInterestService, PointOfInterestService>();

            return services;
        }
    }
}

