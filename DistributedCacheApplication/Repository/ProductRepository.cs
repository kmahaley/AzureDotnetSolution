using DistributedCacheApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DistributedCacheApplication.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDictionary<int, Product> keyValuePairs = new Dictionary<int, Product>();

        private readonly ILogger<ProductRepository> logger;

        public ProductRepository(ILogger<ProductRepository> logger)
        {
            this.logger = logger;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken = default)
        {
            await Task.Delay(2000);
            return keyValuePairs.Values.ToList();
        }

        public async Task<Product> GetProductAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                await Task.Delay(2000);
                var existingEntity = keyValuePairs[id];
                return existingEntity;
            }
            catch (KeyNotFoundException ex)
            {
                logger.LogError($"key element not present {id}");
                throw;
            }
            
        }

        public async Task<Product> AddProductAsync(Product product, CancellationToken cancellationToken = default)
        {
            await Task.Delay(2000);
            var isAdded = keyValuePairs.TryAdd(product.ProductId, product);
            
            if (!isAdded)
            {
                product.LastModifiedDate= DateTime.UtcNow;
                return keyValuePairs[product.ProductId] = product;
            }

            return product;
        }

        public async Task<Product> UpdateProductAsync(int id, Product product, CancellationToken cancellationToken = default)
        {
            await Task.Delay(2000);
            if (!keyValuePairs.ContainsKey(id) && product.ProductId != id)
            {
                throw new ArgumentException($"Element does not exists in the system. {id}");
            }

            product.LastModifiedDate = DateTime.UtcNow;
            keyValuePairs[id] = product;
            return product;
        }

        public async Task DeleteProductAsync(int id, CancellationToken cancellationToken = default)
        {
            await Task.Delay(2000);
            if (!keyValuePairs.ContainsKey(id))
            {
                throw new ArgumentException($"Element does not exists in the system. {id}");
            }

            keyValuePairs.Remove(id);
        }

    }
}
