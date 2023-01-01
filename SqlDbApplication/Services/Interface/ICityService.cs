using SqlDbApplication.Models.Dtos;
using SqlDbApplication.Models.Sql;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SqlDbApplication.Services.Interface
{
    public interface ICityService
    {
        Task<CityDto> AddCityAsync(CityDto city, CancellationToken cancellationToken);
        Task<CityDto> DeleteCityByIdAsync(int id, CancellationToken cancellationToken);
        Task<IList<CityDto>> GetAllCitiesAsync(bool? includePoints, CancellationToken cancellationToken);
        Task<CityDto> GetCityByIdAsync(int id, bool? includePoints, CancellationToken cancellationToken);
        Task<CityDto> UpdateCityAsync(int id, CityDto city, CancellationToken cancellationToken);

        Task<IList<CityDto>> GetAllCitiesFilteredUsingNameAsync(string? name, bool includePoints, CancellationToken cancellationToken);
        Task<IList<CityDto>> GetAllCitiesUsingSearchAsync(
            string? name,
            string? searchQuery,
            bool includePoints,
            CancellationToken cancellationToken);

        Task<IList<CityDto>> GetAllCitiesUsingSearchAndPaginationAsync(
            string? name,
            string? searchQuery,
            bool includePoints,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken);

        Task<CityPageDto> GetAllCitiesWithPaginationMetdadataAsync(
            string name,
            string searchQuery,
            bool includePoints,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken);
    }
}
