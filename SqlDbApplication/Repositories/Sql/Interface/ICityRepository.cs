using SqlDbApplication.Models.Dtos;
using SqlDbApplication.Models.Sql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SqlDbApplication.Repositories.Sql.Interface
{
    public interface ICityRepository
    {
        Task<City> AddCityAsync(City city);
        Task<City> DeleteCityByIdAsync(int id);
        Task<IEnumerable<City>> GetAllCitiesAsync(bool includePoints);
        Task<City> GetCityByIdAsync(int id, bool includePoints);
        Task<City> UpdateCityAsync(int id, City city);

        Task<City> GetCityAsync(int id);

        Task<IEnumerable<City>> GetAllCitiesFilteredUsingNameAsync(string name, bool includePoints);

        Task<IEnumerable<City>> GetAllCitiesUsingSearchAsync(string name, string searchQuery, bool includePoints);
        Task<IEnumerable<City>> GetAllCitiesUsingSearchAndPaginationAsync(
            string name,
            string searchQuery,
            bool includePoints,
            int pageNumber,
            int pageSize);
        
        Task<CityPage> GetAllCitiesWithPaginationMetdadataAsync(
            string name,
            string searchQuery,
            bool includePoints,
            int pageNumber,
            int pageSize);
    }
}
