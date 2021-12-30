using Microsoft.Extensions.DependencyInjection;
using MongoDbApplication.Mapper;

namespace MongoDbApplication.DependencyExtensions
{
    /// <summary>
    /// Add mapper specific instances.
    /// </summary>
    public static class MapperInstances
    {
        public static IServiceCollection AddMapperInstances(this IServiceCollection services)
        {
            //Mapper
            services.AddAutoMapper(typeof(MapperProfile));

            return services;
        }
    }
}
