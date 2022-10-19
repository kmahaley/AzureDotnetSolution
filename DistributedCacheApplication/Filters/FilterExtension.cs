using System.Runtime.CompilerServices;

namespace DistributedCacheApplication.Filters
{
    public static class FilterExtension
    {
        public static IServiceCollection AddApplicationFilters(this IServiceCollection services)
        {
            services.AddSingleton<HttpETagFilter>();
            services.AddSingleton<ControllerFilter>();
            services.AddSingleton<GlobalApplicationFilter>();

            return services;
        }
    }
}
