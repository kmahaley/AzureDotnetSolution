using SqlDbApplication.Models.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SqlDbApplication.Services.Interface
{
    public interface ICityService
    {
        Task<CityDto> AddCityAsync(CityDto city);
        Task<CityDto> DeleteCityByIdAsync(int id);
        Task<IList<CityDto>> GetAllCitiesAsync(bool? includePoints);
        Task<CityDto> GetCityByIdAsync(int id, bool? includePoints);
        Task<CityDto> UpdateCityAsync(int id, CityDto city);

        Task<IList<CityDto>> GetAllCitiesFilteredUsingNameAsync(string? name, bool includePoints);
        Task<IList<CityDto>> GetAllCitiesUsingSearchAsync(string? name, string? searchQuery, bool includePoints);
    }
}
