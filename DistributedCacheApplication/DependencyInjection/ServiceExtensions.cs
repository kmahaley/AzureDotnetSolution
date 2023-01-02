using DistributedCacheApplication.Filters;
using DistributedCacheApplication.Middlewares;
using DistributedCacheApplication.Repository;
using DistributedCacheApplication.Services;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace DistributedCacheApplication.DependencyInjection
{
    public static class ServiceExtensions
    {
        private const string RedisConfigurationName = "Redis";

        public static IServiceCollection AddRepositoryDependencies(
            this IServiceCollection services,
            IConfiguration configuration) 
        {
            services.AddSingleton<IProductRepository, ProductRepository>();

            return services;
        }

        public static IServiceCollection AddInMemoryCacheDependencies(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // in memory caching
            services.AddMemoryCache();

            return services;
        }

        /// <summary>
        /// This type creates DI for IDistributedCache.
        /// easy but restrcited datatypes
        /// </summary>
        public static IServiceCollection AddRedisDependencies(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var redisConnection = configuration.GetConnectionString(RedisConfigurationName); // same as Configuration.GetSection("ConnectionStrings:Redis");
            // distributed caching
            services.AddStackExchangeRedisCache(redisOptions => redisOptions.Configuration = redisConnection);

            return services;
        }

        /// <summary>
        /// this type create DI for IConnectionMultiplexer.
        /// complicated but all datatypes
        /// </summary>
        public static IServiceCollection AddRedisUsingConnectionMultiplexerDependencies(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var redisConnection = configuration.GetConnectionString(RedisConfigurationName); // same as Configuration.GetSection("ConnectionStrings:Redis");
            // distributed caching
            services.AddSingleton<IConnectionMultiplexer>(redisOptions => ConnectionMultiplexer.Connect(redisConnection));

            return services;
        }

        public static IServiceCollection AddServiceDependencies(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<IProductService, ProductService>();

            return services;
        }
        
        public static IServiceCollection AddMiddlewareDependencies(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<HttpEtagMiddleware>();

            return services;
        }
        
        public static IServiceCollection AddApplicationFilterDependencies(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<HttpETagFilter>();
            services.AddSingleton<ControllerFilter>();
            services.AddSingleton<GlobalApplicationFilter>();

            return services;
        }


    }
}
