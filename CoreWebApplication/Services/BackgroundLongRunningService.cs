
using CoreWebApplication.Models;
using CoreWebApplication.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoreWebApplication.Services
{
    public class BackgroundLongRunningService : BackgroundService
    {
        private CancellationTokenSource cancellationTokenSource;

        private readonly ILogger logger;

        private readonly IRepository repository;

        public BackgroundLongRunningService(IRepository repository, ILogger<BackgroundLongRunningService> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }


        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
            try
            {
                await UpsertItemsAsync("ExecuteAsync => UpsertItemsAsync", "banana", stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogCritical("------- exception from while loop and UpsertItemsAsync method");
            }
            
            if (stoppingToken.IsCancellationRequested)
            {
                logger.LogWarning($"===> Long Service stop requested: {stoppingToken.IsCancellationRequested}");
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            logger.LogWarning("===> BackgroundLongRunningService Hosted Service is stopping.");

            await base.StopAsync(stoppingToken);
        }

        private async Task UpsertItemsAsync(string methodName, string name, CancellationToken cancellationToken)
        {
            int i = 0;
            while (!cancellationTokenSource.IsCancellationRequested
                && !cancellationToken.IsCancellationRequested 
                && i < 10)
            {
                try
                {
                    logger.LogInformation($"----- {methodName}: Processing task----------- {i}");
                    if (i == 3)
                    {
                        throw new Exception($"manually fail on exception.... {i}");
                    }
                    var id = Guid.NewGuid();
                    var item = new Item
                    {
                        Id = id,
                        Name = $"{name}_apple",
                        Price = 10,
                        CreatedDate = DateTime.UtcNow
                    };

                    _ = repository.CreateOrUpdateItemAsync(id, item);
                    i++;
                    await Task.Delay(TimeSpan.FromSeconds(3));
                }
                catch (Exception ex)
                {
                    logger.LogError($"{methodName} error occured :(. {ex.GetType().Name}, {ex.Message}");
                    throw;
                }
            }            
        }
    }
}