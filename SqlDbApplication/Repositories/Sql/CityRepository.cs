using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SqlDbApplication.Exceptions;
using SqlDbApplication.Models.Sql;
using SqlDbApplication.Repositories.Sql.Interface;
using SqlDbApplication.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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

        public async Task<IEnumerable<City>> GetAllCitiesAsync(bool includePoints)
        {
            if (includePoints)
            {
                
                return await databaseContext
                    .Cities
                    .Include(city => city.PointOfInterests)
                    .ToListAsync();
            }
            return await databaseContext.Cities.ToListAsync();
        }

        public async Task<City> GetCityByIdAsync(int id)
        {
            var city = await databaseContext.Cities.FindAsync(id);
            if (city == null)
            {
                throw new SqlDbApplicationException(
                    $"City is not present with id {id}",
                    ErrorCode.IncorrectEntityIdProvided);
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
