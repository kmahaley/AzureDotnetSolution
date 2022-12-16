
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
        
        //public BackgroundLongRunningService(IServiceProvider services, ILogger<BackgroundLongRunningService> logger)
        //{
        //    this.repository = services.GetRequiredService<IRepository>();
        //    this.logger = logger;
        //}

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("1. StartAsync has been called.");
            UpsertItems("StartAsync", "apple");
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("4. StopAsync has been called.");
            return Task.CompletedTask;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("finally -----------------");
            UpsertItems("ExecuteAsync", "banana");
            await Task.CompletedTask;
        }

        private void UpsertItems(string methodName, string name)
        {
            try
            {
                logger.LogInformation($"----- {methodName}: Processing task");
                for(int i = 0; i < 2; i++)
                {
                    logger.LogInformation($"{repository == null} {i}");
                    var id = Guid.NewGuid();
                    var item = new Item
                    {
                        Id = id,
                        Name = $"{name}_{i}",
                        Price = 10,
                        CreatedDate = DateTime.UtcNow
                    };

                    _ = repository.CreateOrUpdateItemAsync(id, item);
                    Thread.Sleep(TimeSpan.FromSeconds(2));
                }
                logger.LogInformation($"----- {methodName}: Completed task");
            }
            catch(Exception ex)
            {
                logger.LogError($"{methodName} error occured. {ex.GetType().Name}, {ex.Message}");
                logger.LogError($"{methodName} error occured. {ex}");
            }
        }
    }
}