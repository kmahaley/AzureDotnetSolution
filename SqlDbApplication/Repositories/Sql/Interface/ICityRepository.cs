using SqlDbApplication.Models.Dtos;
using SqlDbApplication.Models.Sql;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SqlDbApplication.Repositories.Sql.Interface
{
    public interface ICityRepository
    {
        Task<City> AddCityAsync(City city, CancellationToken cancellationToken);

        Task<City> DeleteCityByIdAsync(int id, CancellationToken cancellationToken);

        Task<IEnumerable<City>> GetAllCitiesAsync(bool includePoints, CancellationToken cancellationToken);

        Task<City> GetCityByIdAsync(
            int id,
            bool includePoints,
            CancellationToken cancellationToken);

        Task<City> UpdateCityAsync(int id, City city, CancellationToken cancellationToken);

        Task<City> GetCityAsync(int id, CancellationToken cancellationToken);

        Task<IEnumerable<City>> GetAllCitiesFilteredUsingNameAsync(
            string name,
            bool includePoints,
            CancellationToken cancellationToken);

        Task<IEnumerable<City>> GetAllCitiesUsingSearchAsync(
            string name,
            string searchQuery,
            bool includePoints,
            CancellationToken cancellationToken);

        Task<IEnumerable<City>> GetAllCitiesUsingSearchAndPaginationAsync(
            string name,
            string searchQuery,
            bool includePoints,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken);
        
        Task<CityPage> GetAllCitiesWithPaginationMetdadataAsync(
            string name,
            string searchQuery,
            bool includePoints,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken);
    }
}
