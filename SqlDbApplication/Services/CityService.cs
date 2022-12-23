using SqlDbApplication.Models.Sql;
using SqlDbApplication.Services.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SqlDbApplication.Services
{
    public class CityService : ICityService
    {
        public Task<City> AddCityAsync(City city)
        {
            throw new System.NotImplementedException();
        }

        public Task<City> DeleteCityByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IList<City>> GetAllCitiesAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<City> GetCityByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<City> UpdateCityAsync(int id, City city)
        {
            throw new System.NotImplementedException();
        }
    }
}
