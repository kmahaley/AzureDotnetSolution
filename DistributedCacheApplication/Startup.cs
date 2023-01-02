using DistributedCacheApplication.DependencyInjection;
using DistributedCacheApplication.Filters;
using DistributedCacheApplication.Middlewares;
using DistributedCacheApplication.Repository;
using DistributedCacheApplication.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace DistributedCacheApplication
{
    public class Startup
    {
        private const string SwaggerApiName = "Distributed application API";

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddInMemoryCacheDependencies(Configuration)
                .AddRedisDependencies(Configuration)
                .AddRedisUsingConnectionMultiplexerDependencies(Configuration)
                .AddServiceDependencies(Configuration)
                .AddRepositoryDependencies(Configuration)
                .AddMiddlewareDependencies(Configuration)
                .AddApplicationFilterDependencies(Configuration);

            //Controller
            services.AddControllers();
            //services.AddControllers(options =>
            //{
            //    options.Filters.AddService(typeof(GlobalApplicationFilter));
            //});

            services.AddHealthChecks();

            //Swagger
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new OpenApiInfo()
                    {
                        Title = SwaggerApiName,
                        Version = "v1",
                        Description = "Use this application to test Redis cache"
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthorization();

            // can add middleware here
            //app.UseMiddleware<HttpEtagMiddleware>();

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
