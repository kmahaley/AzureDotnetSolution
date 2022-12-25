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

        public Task<IList<CityDto>> GetAllCitiesAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<CityDto> GetCityByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<CityDto> UpdateCityAsync(int id, CityDto city)
        {
            throw new System.NotImplementedException();
        }
    }
}
