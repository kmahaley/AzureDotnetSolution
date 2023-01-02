using DistributedCacheApplication.Models;

namespace DistributedCacheApplication.Services
{
    public class RedisCacheService : ICacheService
    {
        public Task<Product> AddValueAsync(int cacheKey, Product product, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Product>> GetAllValuesAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetValueAsync(int cacheKey, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveValueAsync(int cacheKey, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
