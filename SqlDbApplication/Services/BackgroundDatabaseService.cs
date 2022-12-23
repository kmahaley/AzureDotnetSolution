using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SqlDbApplication.Models.Sql;
using SqlDbApplication.Repositories.Sql.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SqlDbApplication.Services
{
    public class BackgroundDatabaseService : BackgroundService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<BackgroundDatabaseService> logger;

        public BackgroundDatabaseService(IServiceProvider serviceProvider, ILogger<BackgroundDatabaseService> logger)
        {
            this.serviceProvider = serviceProvider;
            this.logger = logger;
        }

        /// <summary>
        /// Runs all the task as background tasks. 
        /// </summary>
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation($"-------- finally {nameof(BackgroundDatabaseService)} is running.");
            using IServiceScope scope = serviceProvider.CreateScope();
            var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
            var existingProduct = await productRepository.GetProductByIdAsync(1);
            logger.LogInformation($"---- {nameof(BackgroundDatabaseService)} Got product. {existingProduct.AvailableQuantity}");
            
            for(int i = 0; i < 5; i++)
            {
                existingProduct.AvailableQuantity = existingProduct.AvailableQuantity + (i * 10);
                Thread.Sleep(TimeSpan.FromSeconds(4));
                logger.LogInformation($"---- {nameof(BackgroundDatabaseService)} updating. {existingProduct.AvailableQuantity }");
                await productRepository.UpdateProductAsync(existingProduct.ProductId, existingProduct);
            }

            await Task.CompletedTask;
        }
    }
}
