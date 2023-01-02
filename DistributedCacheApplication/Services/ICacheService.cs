using DistributedCacheApplication.Models;

namespace DistributedCacheApplication.Services
{
    public interface ICacheService
    {
        public Task<Product> GetValueAsync(int cacheKey);

        public Task<bool> TryGetValueAsync(int cacheKey, out Product savedValue);

        public Task AddValueAsync(int cacheKey, Product product);

        public Task AddValueAsync(int cacheKey, Product product, TimeSpan expiration);

        public Task RemoveValueAsync(int cacheKey);

    }
}
