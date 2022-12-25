using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SqlDbApplication.Models.Sql;
using SqlDbApplication.Repositories.Sql.Interface;
using SqlDbApplication.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SqlDbApplication.Repositories.Sql
{
    public class CityRepository : ICityRepository
    {
        private readonly SqlDatabaseContext databaseContext;

        private readonly ILogger<CityRepository> logger;

        public CityRepository(SqlDatabaseContext databaseContext, ILogger<CityRepository> logger) 
        {
            this.databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<City> AddCityAsync(City city)
        {
            var savedCity = await databaseContext.Cities.AddAsync(city);
            await databaseContext.SaveChangesAsync();
            return savedCity.Entity;
        }

        public async Task<City> DeleteCityByIdAsync(int id)
        {
            var existingCity = await GetCityByIdAsync(id);
            databaseContext.Cities.Remove(existingCity);
            await databaseContext.SaveChangesAsync();
            return existingCity;
        }

        public async Task<IEnumerable<City>> GetAllCitiesAsync()
        {
            var list = await databaseContext.Cities.ToListAsync();
            return list;
        }

        public async Task<City> GetCityByIdAsync(int id)
        {
            var city = await databaseContext.Cities.FindAsync(id);
            if (city == null)
            {
                throw new ArgumentException($"City is not present with id {id}");
            }
            return city;
        }

        public async Task<City> UpdateCityAsync(int id, City city)
        {
            var existingCity = await GetCityByIdAsync(id);
            existingCity.Name = city.Name;
            existingCity.Population = city.Population;
            existingCity.Description = city.Description;
            await databaseContext.SaveChangesAsync();
            return city;
        }
    }
}
