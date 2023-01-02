using DistributedCacheApplication.Filters;
using DistributedCacheApplication.Middlewares;
using DistributedCacheApplication.Repository;
using DistributedCacheApplication.Services;
using Microsoft.Extensions.Configuration;

namespace DistributedCacheApplication.DependencyInjection
{
    public static class ServiceExtensions
    {

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
        
        public static IServiceCollection AddDistributedCacheDependencies(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Redis"); // same as Configuration.GetSection("ConnectionStrings:Redis");
            // distributed caching
            services.AddStackExchangeRedisCache(redisOptions => redisOptions.Configuration = connectionString);

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
