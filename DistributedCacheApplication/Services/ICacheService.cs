using DistributedCacheApplication.Models;

namespace DistributedCacheApplication.Services
{
    public interface ICacheService
    {
        public Task<Product> GetValueAsync(int cacheKey, CancellationToken cancellationToken);

        public Task<Product> AddValueAsync(int cacheKey, Product product, CancellationToken cancellationToken);

        public Task RemoveValueAsync(int cacheKey, CancellationToken cancellationToken);

        public Task<IList<Product>> GetAllValuesAsync(CancellationToken cancellationToken);

    }
}
