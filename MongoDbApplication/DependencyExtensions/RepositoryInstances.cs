using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using MongoDbApplication.Configurations;
using MongoDbApplication.Repositories;

namespace MongoDbApplication.DependencyExtensions
{
    /// <summary>
    /// Add repository specific instances.
    /// </summary>
    public static class RepositoryInstances
    {
        public static IServiceCollection AddRepositoryInstances(this IServiceCollection services)
        {
            //Serialize item's properties in Mongodb
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

            //Repositories
            services.AddSingleton<IMongoClient>(serviceProvider =>
            {
                var mongoDbConfigurationOptions = serviceProvider.GetService<IOptions<MongoDbConfiguration>>();
                var mongoDbConfiguration = mongoDbConfigurationOptions.Value;
                return new MongoClient(mongoDbConfiguration.ConnectionString);
            });

            services.AddSingleton<IRepository, MongoDbRepository>();

            //services.AddSingleton<IRepository, InMemoryRepository>();

            return services;
        }
    }
}
