using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using MongoDbApplication.Configurations;
using MongoDbApplication.Repositories;

namespace MongoDbApplication
{
    public class Startup
    {
        private const string SwaggerApiName = "MongoDb application API";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //Configurations
            services.Configure<MongoDbConfiguration>(Configuration.GetSection("MongoDbConfiguration"));
            var mongoDbConfiguration = Configuration.GetSection("MongoDbConfiguration").Get<MongoDbConfiguration>();

            //Serialize item's properties in Mongodb
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

            //Repositories
            services.AddSingleton<IMongoClient>(serviceProvider => {
                return new MongoClient(mongoDbConfiguration.ConnectionString);
            });
            services.AddSingleton<IRepository, MongoDbRepository>();
            //services.AddSingleton<IRepository, InMemoryRepository>();

            services.AddControllers();
            services.AddHealthChecks();

            //Swagger
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new OpenApiInfo()
                    {
                        Title = SwaggerApiName,
                        Version = "v1",
                        Description = "Use this application to learn Mongo and Devops"
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", SwaggerApiName);
            });
        }
    }
}
