using CoreWebApplication.Models;
using CoreWebApplication.Repositories;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoreWebApplication.Services
{
    public class BackgroundTimedRunningService : IHostedService
    {
        private Timer? timer = null;

        private readonly ILogger<BackgroundTimedRunningService> logger;

        //private readonly IServiceProvider services;

        private readonly IRepository repository;
        
        public BackgroundTimedRunningService(IRepository repository, ILogger<BackgroundTimedRunningService> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Timed Hosted Service running.");

            timer = new Timer(UpsertItemsAsync, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer.Change(Timeout.Infinite, 0);
            timer?.Dispose();
            return Task.CompletedTask;
        }

        private void UpsertItemsAsync(object? state)
        {
            while (true)
            {
                try
                {
                    logger.LogInformation($"----- UpsertItemsAsync: Processing task");
                    var id = Guid.NewGuid();
                    var item = new Item
                    {
                        Id = id,
                        Name = "banana_apple",
                        Price = 10,
                        CreatedDate = DateTime.UtcNow
                    };

                    _ = repository.CreateOrUpdateItemAsync(id, item);
                    //await Task.Delay(TimeSpan.FromSeconds(3));
                }
                catch (Exception ex)
                {
                    logger.LogError($"error occured. {ex.GetType().Name}, {ex.Message}");
                    logger.LogError($"error occured. {ex}");
                }
            }
        }
    }
}
