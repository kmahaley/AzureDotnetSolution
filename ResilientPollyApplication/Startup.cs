using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ResilientPollyApplication.Extensions;
using ResilientPollyApplication.Handlers;
using ResilientPollyApplication.Services;
using System;

namespace ResilientPollyApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            this.env = env;
        }

        public IConfiguration Configuration
        {
            get;
        }
        private readonly IWebHostEnvironment env;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if(env.IsDevelopment())
            {
                Console.WriteLine($"-------- {env.EnvironmentName}");
            }
            
            services.AddControllers();
            services.AddSingleton<TimingHttpMessageHandler>();
            

            services.AddPollyWrappedBasedDependencies();

            services.AddHttpNamedBasedDependencies();

            services.AddHttpTypedBasedDependencies();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
