using DistributedCacheApplication.Models;
using Microsoft.Extensions.Caching.Memory;

namespace DistributedCacheApplication.Services
{
    public class InMemoryCacheService : ICacheService
    {
        private readonly IMemoryCache memoryCache;

        private readonly ILogger<InMemoryCacheService> logger;

        public InMemoryCacheService(ILogger<InMemoryCacheService> logger, IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<Product> GetValueAsync(int cacheKey)
        {
            var Product = memoryCache.Get<Product>(cacheKey);
            return Task.FromResult(Product);
        }

        public Task<bool> TryGetValueAsync(int cacheKey, out Product savedValue)
        {
            var isProductFound = memoryCache.TryGetValue(cacheKey, out Product existingProduct);
            savedValue = existingProduct;
            return Task.FromResult(isProductFound);
        }

        public Task AddValueAsync(int cacheKey, Product product)
        {
            try
            {
                memoryCache.Set(cacheKey, product);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }

            return Task.CompletedTask;
        }

        public Task AddValueAsync(int cacheKey, Product product, TimeSpan expiration)
        {
            try
            {
                memoryCache.Set(cacheKey, product, expiration);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }

            return Task.CompletedTask;
        }

        public Task RemoveValueAsync(int cacheKey)
        {
            try
            {
                memoryCache.Remove(cacheKey);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }

            return Task.CompletedTask;
        }
    }
}
