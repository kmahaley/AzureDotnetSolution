using CoreWebApplication.Repositories;
using CoreWebApplication.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;

namespace CoreWebApplication
{
    public class Startup
    {
        private const string SwaggerApiName = "Web application API";

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Serilog configuration, uncomment .UseSerilog() in program.cs
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("Logs/CoreWebApplication.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            // Configurations
           
            // Repositories
            services.AddSingleton<IRepository, InMemoryRepository>();

            // Background Services
            //services.AddSingleton<IHostedService, BackgroundShortRunningService>();
            //services.AddHostedService<BackgroundLongRunningService>();
            //services.AddHostedService<BackgroundTimedRunningService>();

            // Controller
            services.AddControllers();
            services.AddHealthChecks();


            // Swagger
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new OpenApiInfo()
                    {
                        Title = SwaggerApiName,
                        Version = "v1",
                        Description = "Use this application to .Net application"
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