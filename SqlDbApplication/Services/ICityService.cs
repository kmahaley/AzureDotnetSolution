using SqlDbApplication.Models.Sql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SqlDbApplication.Services
{
    public interface ICityService
    {
        Task<City> AddCityAsync(City city);
        Task<City> DeleteCityByIdAsync(int id);
        Task<IList<City>> GetAllCitiesAsync();
        Task<City> GetCityByIdAsync(int id);
        Task<City> UpdateCityAsync(int id, City city);
    }
}
