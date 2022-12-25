using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SqlDbApplication.Exceptions;
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

        private const int ElementId = 1;

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
            logger.LogInformation($"<---> {nameof(BackgroundDatabaseService)} service is running. <---> ");
            using IServiceScope scope = serviceProvider.CreateScope();
            var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
            
            Product existingProduct = new Product();            ;
            bool isPresent = true;
            try
            {
                existingProduct = await productRepository.GetProductByIdAsync(ElementId);
            }
            catch (SqlDbApplicationException ex) 
            {
                logger.LogError("Element:{ElementId} not found in the database. code:{errorCode}. Not moving with the update call.", ElementId, ex.ErrorCode);
                isPresent = false;
                //throw;
            }
            
            if (isPresent)
            {
                logger.LogInformation($"---> {nameof(BackgroundDatabaseService)} Got product. {existingProduct.AvailableQuantity}");

                for (int i = 0; i < 5; i++)
                {
                    existingProduct.AvailableQuantity = existingProduct.AvailableQuantity + (i * 10);
                    Thread.Sleep(TimeSpan.FromSeconds(4));
                    logger.LogInformation($"---> {nameof(BackgroundDatabaseService)} updating. {existingProduct.AvailableQuantity}");
                    await productRepository.UpdateProductAsync(existingProduct.ProductId, existingProduct);
                }
            }
            

            await Task.CompletedTask;
        }
    }
}
