using SqlDbApplication.Models.Sql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SqlDbApplication.Repositories.Sql.Interface
{
    public interface ICityRepository
    {
        Task<City> AddCityAsync(City city);
        Task<City> DeleteCityByIdAsync(int id);
        Task<IEnumerable<City>> GetAllCitiesAsync();
        Task<City> GetCityByIdAsync(int id);
        Task<City> UpdateCityAsync(int id, City city);
    }
}
