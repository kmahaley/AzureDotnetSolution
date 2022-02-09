using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace NoHttpWorkloadApplication.Services
{
    public class BackgroundHostedBookService : IHostedService
    {
        private readonly ILogger logger;

        public BackgroundHostedBookService(ILogger<BackgroundHostedBookService> logger, IHostApplicationLifetime appLifetime)
        {
            this.logger = logger;

            appLifetime.ApplicationStarted.Register(OnStarted);
            appLifetime.ApplicationStopping.Register(OnStopping);
            appLifetime.ApplicationStopped.Register(OnStopped);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("1. StartAsync has been called.");
            for(int i = 0; i < 2; i++)
            {
                logger.LogInformation($"StartAsync : {i}");
                //Thread.Sleep(TimeSpan.FromSeconds(2));
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("4. StopAsync has been called.");
            for(int i = 0; i < 2; i++)
            {
                logger.LogInformation($"StopAsync : {i}");
                //Thread.Sleep(TimeSpan.FromSeconds(2));
            }

            return Task.CompletedTask;
        }

        private void OnStarted()
        {
            logger.LogInformation("2. OnStarted has been called.");
        }

        private void OnStopping()
        {
            logger.LogInformation("3. OnStopping has been called.");
        }


        private void OnStopped()
        {
            logger.LogInformation("5. OnStopped has been called.");
        }
    }
}