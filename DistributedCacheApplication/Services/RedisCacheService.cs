using DistributedCacheApplication.Models;

namespace DistributedCacheApplication.Services
{
    public class RedisCacheService
    {
        public Task<Employee> GetCacheAsync(int cacheKey)
        {
            throw new NotImplementedException();
        }

        public Task SetCacheAsync(int cacheKey, Employee employee)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Employee>> GetAllCacheDataAsync() 
        { 
            throw new NotImplementedException();
        }
    }
}
