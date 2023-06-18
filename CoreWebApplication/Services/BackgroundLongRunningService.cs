
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
                await UpsertItemsAsync("banana", stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogCritical("===> exception from long running task: UpsertItemsAsync method");
                throw;
            }
            
            logger.LogInformation("long running task completed.");
            
            if (stoppingToken.IsCancellationRequested)
            {
                logger.LogCritical("------------------------- Long Service stop requested");
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("BackgroundLongRunningService Hosted Service is stopping.");

            await base.StopAsync(stoppingToken);
        }

        private async Task UpsertItemsAsync(string name, CancellationToken cancellationToken)
        {
            int i = 0;
            try
            {
                while (!cancellationTokenSource.IsCancellationRequested
                && !cancellationToken.IsCancellationRequested
                && i < 10)
                {

                    logger.LogInformation($"---Processing task ----------- {i}");
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
                    await Task.Delay(TimeSpan.FromSeconds(1));

                }
            }
            catch (Exception ex)
            {
                logger.LogError($"--- Error occured in long running task. {ex.GetType().Name}, {ex.Message}");
                //this.StopAsync(cancellationToken);
                throw;
            }
        }
    }
}