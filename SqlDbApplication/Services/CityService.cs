using AutoMapper;
using Microsoft.Extensions.Logging;
using SqlDbApplication.Models.Dtos;
using SqlDbApplication.Models.Sql;
using SqlDbApplication.Repositories.Sql.Interface;
using SqlDbApplication.Services.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SqlDbApplication.Services
{
    public class CityService : ICityService
    {
        private readonly ICityRepository cityRepository;

        private readonly ILogger<CityService> logger;

        private readonly IMapper mapper;

        public CityService(ICityRepository cityRepository, ILogger<CityService> logger, IMapper mapper)
        {
            this.cityRepository = cityRepository;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task<CityDto> AddCityAsync(CityDto cityDto)
        {
            var city = mapper.Map<City>(cityDto);
            var savedCity = await cityRepository.AddCityAsync(city);
            return mapper.Map<CityDto>(savedCity);
        }

        public async Task<CityDto> DeleteCityByIdAsync(int id)
        {
            var city = await cityRepository.DeleteCityByIdAsync(id);
            return mapper.Map<CityDto>(city);
        }

        public async Task<IList<CityDto>> GetAllCitiesAsync(bool? includePoints)
        {
            bool isIncludePointOfInterest = false;
            if (includePoints.HasValue && includePoints.Value)
            {
                isIncludePointOfInterest = true;
            }
            var cities = await cityRepository.GetAllCitiesAsync(isIncludePointOfInterest);
            var listOfCities = mapper.Map<IEnumerable<City>, List<CityDto>>(cities);
            return listOfCities;
        }

        public async Task<CityDto> GetCityByIdAsync(int id, bool? includePoints)
        {
            bool isIncludePointOfInterest = false;
            if (includePoints.HasValue && includePoints.Value == true)
            {
                isIncludePointOfInterest = true;
            }
            var city = await cityRepository.GetCityByIdAsync(id, isIncludePointOfInterest);
            return mapper.Map<CityDto>(city);
        }

        public async Task<CityDto> UpdateCityAsync(int id, CityDto cityDto)
        {
            var newCity = mapper.Map<City>(cityDto);
            var updatedCity = await cityRepository.UpdateCityAsync(id, newCity);
            return mapper.Map<CityDto>(updatedCity);
        }

        public async Task<IList<CityDto>> GetAllCitiesFilteredUsingNameAsync(string? name, bool includePoints) 
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return await GetAllCitiesAsync(includePoints);
            }
            else
            {
                name = name.Trim();
                var cities = await cityRepository.GetAllCitiesFilteredUsingNameAsync(name, includePoints);
                var listOfCities = mapper.Map<IEnumerable<City>, List<CityDto>>(cities);
                return listOfCities;
            }
            
        }
        
        public async Task<IList<CityDto>> GetAllCitiesUsingSearchAsync(string? name, string? searchQuery, bool includePoints) 
        {
            if (string.IsNullOrWhiteSpace(name)
                && string.IsNullOrWhiteSpace(searchQuery))
            {
                return await GetAllCitiesAsync(includePoints);
            }

            var cities = await cityRepository.GetAllCitiesUsingSearchAsync(name, searchQuery, includePoints);
            return mapper.Map<IEnumerable<City>, List<CityDto>>(cities);
        }
    }
}
