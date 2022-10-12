using DistributedCacheApplication.Models;
using Microsoft.Extensions.Caching.Memory;

namespace DistributedCacheApplication.Services
{
    public class InMemoryCacheService : ICacheService
    {
        private readonly MemoryCache memoryCache;

        public InMemoryCacheService()
        {
            memoryCache = new MemoryCache(new MemoryCacheOptions());
        }

        public Task<Employee> GetCacheAsync(int cacheKey)
        {
            var employee = memoryCache.Get<Employee>(cacheKey);
            return Task.FromResult(employee);
        }

        public Task SetCacheAsync(int cacheKey, Employee employee)
        {
            memoryCache.Set(cacheKey, employee);
            return Task.CompletedTask;
        }

        public Task<IList<Employee>> GetAllCacheDataAsync() 
        {
            throw new NotImplementedException();
        }
    }
}
