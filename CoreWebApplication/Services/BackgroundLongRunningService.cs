
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
        private readonly ILogger logger;

        private readonly IServiceProvider services;

        private readonly IRepository repository;

        public BackgroundLongRunningService(IRepository repository, ILogger<BackgroundLongRunningService> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }


        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await UpsertItemsAsync("ExecuteAsync => UpsertItemsAsync", "banana", stoppingToken);
            if (stoppingToken.IsCancellationRequested)
            {
                logger.LogError("------------------------- Long Service stop requested");
            }
            //stoppingToken.ThrowIfCancellationRequested();
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("BackgroundLongRunningService Hosted Service is stopping.");

            await base.StopAsync(stoppingToken);
        }

        private async Task UpsertItemsAsync(string methodName, string name, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    logger.LogInformation($"----- {methodName}: Processing task");
                    var id = Guid.NewGuid();
                    var item = new Item
                    {
                        Id = id,
                        Name = $"{name}_apple",
                        Price = 10,
                        CreatedDate = DateTime.UtcNow
                    };

                    _ = repository.CreateOrUpdateItemAsync(id, item);
                    await Task.Delay(TimeSpan.FromSeconds(3));
                }
                catch (Exception ex)
                {
                    logger.LogError($"{methodName} error occured. {ex.GetType().Name}, {ex.Message}");
                    logger.LogError($"{methodName} error occured. {ex}");
                }
            }            
        }
    }
}