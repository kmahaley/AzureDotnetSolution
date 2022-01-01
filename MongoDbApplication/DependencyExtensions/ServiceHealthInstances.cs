using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB;
using Microsoft.Extensions.Configuration;
using MongoDbApplication.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MongoDbApplication.DependencyExtensions
{
    public static class ServiceHealthInstances
    {
        public static IServiceCollection AddServiceHealthInstances(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var mongoDbConfigurationOptions = serviceProvider.GetRequiredService<IOptions<MongoDbConfiguration>>();
            var mongoDbConfiguration = mongoDbConfigurationOptions.Value;

            services.AddHealthChecks()
                .AddMongoDb(
                    mongoDbConfiguration.ConnectionString,
                    name: "MongoDbDatabase",
                    tags: new string[] { "ready" },
                    timeout: TimeSpan.FromSeconds(3));
            return services;
        }
    }
}
