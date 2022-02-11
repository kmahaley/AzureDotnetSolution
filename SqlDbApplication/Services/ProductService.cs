﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SqlDbApplication.Models.Sql;
using SqlDbApplication.Repositories.Sql;

namespace SqlDbApplication.Services
{
    public class ProductService : IProductService
    {
        private readonly ILogger<ProductService> logger;

        private readonly IFireAndForgetService fireAndForgetService;

        private readonly IServiceProvider serviceProvider;

        public ProductService(
            IServiceProvider serviceProvider,
            ILogger<ProductService> logger,
            IFireAndForgetService fireAndForgetService)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.fireAndForgetService = fireAndForgetService;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            using IServiceScope scope = serviceProvider.CreateScope();
            IProductRepository productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
            return await productRepository.GetAllProductsAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            using IServiceScope scope = serviceProvider.CreateScope();
            IProductRepository productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
            return await productRepository.GetProductByIdAsync(id);
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            using IServiceScope scope = serviceProvider.CreateScope();
            IProductRepository productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
            return await productRepository.AddProductAsync(product);
        }

        public async Task<Product> UpdateProductAsync(int id, Product product)
        {
            using IServiceScope scope = serviceProvider.CreateScope();
            IProductRepository productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
            return await productRepository.UpdateProductAsync(id, product);
        }

        public async Task<Product> DeleteProductByIdAsync(int id)
        {
            using IServiceScope scope = serviceProvider.CreateScope();
            IProductRepository productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
            return await productRepository.DeleteProductByIdAsync(id);
        }

        /// <summary>
        /// This method will throw exception: Purposefully done
        /// This API demonstrates, API is adding data and updating in a FireAndForget manner on a new thread.
        /// - DbContext is scoped instance hence primary thread adds data.
        /// - new thread does not have DbContext scope instance hence update fails.
        /// </summary>
        public async Task<Product> DisposeContextIssueAsync(Product product)
        {
            logger.LogInformation("Adding data.---");
            IProductRepository productRepository;
            Product savedProduct;
            using(IServiceScope scope = serviceProvider.CreateScope())
            {
                productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
                savedProduct = await productRepository.AddProductAsync(product);
                logger.LogInformation("Added data.---");
            }
            /*
             * Cannot access a disposed context instance. A common cause of this error is disposing a context instance that
             * was resolved from dependency injection and then later trying to use the same context instance
             * elsewhere in your application.
             */
            _ = Task.Run(async () =>
                {
                    try
                    {
                        Product productToBeUpdated = new Product
                        {
                            ProductId = product.ProductId,
                            Name = "movie",
                            UnitPrice = 1200,
                            AvailableQuantity = 12,
                            Color = "Green"
                        };
                        var updated = await productRepository.UpdateProductAsync(product.ProductId, productToBeUpdated);
                        logger.LogInformation($"updated async product data. {updated.Color}---");
                    }
                    catch(Exception ex)// InvalidOperationException: Context is disposed and second thread trying to access
                    {
                        logger.LogError($"error in updating. {ex.Message}");
                    }
                });

            return savedProduct;
        }

        public async Task<Product> SolveDisposeContextIssueAsync(Product product)
        {

            logger.LogInformation("Adding data.---");
            using IServiceScope scope = serviceProvider.CreateScope();
            IProductRepository productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
            var savedProduct = await productRepository.AddProductAsync(product);
            logger.LogInformation("Added data.---");

            Product productToBeUpdated = new Product
            {
                ProductId = product.ProductId,
                Name = "movie",
                UnitPrice = 1200,
                AvailableQuantity = 12,
                Color = "Green"
            };

            Func<IProductRepository, Task> jobFunction =
                (reporsitory) => reporsitory.UpdateProductAsync(product.ProductId, productToBeUpdated);
            fireAndForgetService.ExecuteFireAndForgetJob(jobFunction);

            return savedProduct;
        }
    }
}