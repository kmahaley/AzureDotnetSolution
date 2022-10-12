using DistributedCacheApplication.Models;

namespace DistributedCacheApplication.Services
{
    public interface ICacheService
    {
        public Task<Employee> GetCacheAsync(int cacheKey);

        public Task SetCacheAsync(int cacheKey, Employee employee);

        public Task<IList<Employee>> GetAllCacheDataAsync();
    }
}
