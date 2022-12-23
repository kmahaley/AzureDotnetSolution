using SqlDbApplication.Models.Sql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SqlDbApplication.Repositories.Sql.Interface
{
    public interface IStoreRepository
    {
        Task<Store> AddStoreAsync(Store store);
        Task<Store> DeleteStoreByIdAsync(int id);
        Task<IEnumerable<Store>> GetAllStoresAsync();
        Task<Store> GetStoreByIdAsync(int id);
        Task<Store> UpdateStoreAsync(int id, Store store);
    }
}
