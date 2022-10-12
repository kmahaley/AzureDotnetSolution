using DistributedCacheApplication.Middlewares;
using DistributedCacheApplication.Repository;
using DistributedCacheApplication.Services;

namespace DistributedCacheApplication
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}


