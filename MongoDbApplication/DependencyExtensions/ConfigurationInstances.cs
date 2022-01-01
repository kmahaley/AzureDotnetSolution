using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDbApplication.Configurations;

namespace MongoDbApplication.DependencyExtensions
{
    /// <summary>
    /// Add configuration specific instances.
    /// </summary>
    public static class ConfigurationInstances
    {
        public static IServiceCollection AddConfigurationInstances(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<MongoDbConfiguration>(configuration.GetSection(nameof(MongoDbConfiguration)));

            return services;
        }
    }
}
