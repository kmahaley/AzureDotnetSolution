using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NoHttpWorkloadApplication.Services;

namespace NoHttpWorkloadApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseEnvironment("Development")
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<BackgroundHostedBookService>();
                    services.AddHostedService<BackgroundBookService>();
                    //services.AddSingleton<IHostedService, BackgroundBookService>();
                });
    }
}