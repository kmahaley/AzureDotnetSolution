
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

        public BackgroundLongRunningService(IServiceProvider services, ILogger<BackgroundLongRunningService> logger, IHostApplicationLifetime appLifetime)
        {
            this.repository = services.GetRequiredService<IRepository>();
            this.logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("1. StartAsync has been called.");

            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("4. StopAsync has been called.");

            return Task.CompletedTask;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("----- BookService: Processing task");
            for(int i = 0; i < 2; i++)
            {
                var id = Guid.NewGuid();
                var item = new Item
                {
                    Id = id,
                    Name = $"apple_{i}",
                    Price = 10,
                    CreatedDate = DateTime.UtcNow
                };

                _ = repository.QuickUpdateItemAsync(id, item);
                Thread.Sleep(TimeSpan.FromSeconds(2));
            }
            logger.LogInformation("----- BookService: Completed task");
            return Task.CompletedTask;
        }

        
    }
}