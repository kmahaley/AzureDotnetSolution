using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDbApplication.DependencyExtensions;
using System;
using System.Linq;
using System.Net.Mime;
using System.Text.Json;

namespace MongoDbApplication
{
    public class Startup
    {
        private const string SwaggerApiName = "MongoDb application API";
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Added category specific instances
            services.AddConfigurationInstances(Configuration);
            services.AddRepositoryInstances();
            services.AddMapperInstances();
            services.AddServiceHealthInstances();

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

                //Check if all health checks are OK
                endpoints.MapHealthChecks("/health");

                //Check if all health checks with tag "ready" is working
                endpoints.MapHealthChecks(
                    "/health/ready",
                    new HealthCheckOptions()
                    {
                        Predicate = (check) => check.Tags.Contains("ready"),
                        ResponseWriter = async (context, report) =>
                        {
                            var result = JsonSerializer.Serialize(
                                new
                                {
                                    status = report.Status.ToString(),
                                    checks = report.Entries.Select(entry => new
                                    {
                                        name = entry.Key,
                                        status = entry.Value.Status.ToString(),
                                        exception = entry.Value.Exception != null ? entry.Value.Exception.Message : "none",
                                        duration = entry.Value.Duration.ToString(),
                                        tags = String.Join(", ", entry.Value.Tags.ToArray()),
                                        description = entry.Value.Description

                                    })
                                });
                            context.Response.ContentType = MediaTypeNames.Application.Json;
                            await context.Response.WriteAsync(result);
                        }
                    });
            });

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", SwaggerApiName);
            });
        }
    }
}
