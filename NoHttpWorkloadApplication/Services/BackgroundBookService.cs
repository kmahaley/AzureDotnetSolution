using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NoHttpWorkloadApplication.Services
{
    public class BackgroundBookService : BackgroundService
    {
        private readonly ILogger logger;

        public BackgroundBookService(ILogger<BackgroundBookService> logger)
        {
            this.logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation($"1. ------------- {GetType().Name} StartAsync has been called.");
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation($"3. ------------- {GetType().Name} StopAsync has been called.");
            return Task.CompletedTask;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            { 
                logger.LogInformation($"2. ------------- {GetType().Name} ExecuteAsync has been called.");
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
            
        }


        //protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        //{
        //    await Task.Delay(TimeSpan.FromSeconds(3));
        //    logger.LogInformation($"2. ------------- {GetType().Name} ExecuteAsync has been called.");

        //}
    }
}