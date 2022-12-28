using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SqlDbApplication.Exceptions;
using SqlDbApplication.Models.Sql;
using SqlDbApplication.Repositories.Sql.Interface;
using SqlDbApplication.Services;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var existingCity = await GetCityAsync(id);

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

        public async Task<City> GetCityByIdAsync(int id, bool includePoints)
        {
            if(!includePoints)
            {
                return await GetCityAsync(id);
            }

            var city = await databaseContext
                    .Cities
                    .Where(c => c.CityId == id)
                    .Include(c => c.PointOfInterests)
                    .FirstOrDefaultAsync();

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
            var existingCity = await GetCityAsync(id);
            existingCity.Name = city.Name;
            existingCity.Population = city.Population;
            existingCity.Description = city.Description;
            await databaseContext.SaveChangesAsync();
            return city;
        }

        public async Task<City> GetCityAsync(int id)
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

        public async Task<IEnumerable<City>> GetAllCitiesFilteredUsingNameAsync(string name, bool includePoints) 
        {
            if (includePoints)
            {
                return await databaseContext
                .Cities
                .Include(city => city.PointOfInterests)
                .Where(city => city.Name == name)
                .ToListAsync();
            }

            return await databaseContext
                .Cities
                .Where(city => city.Name == name)
                .ToListAsync();

        }

        /// <summary>
        /// this is using IQuery which is appended based on user requirement and execute only when terminal
        /// expression is called like ".ToListAsync()"
        /// Deferred execution
        /// </summary>
        public async Task<IEnumerable<City>> GetAllCitiesUsingSearchAsync(string name, string searchQuery, bool includePoints)
        {

            var queryCollection = CreateFilterAndSearchQuery(name, searchQuery, includePoints);

            return await queryCollection
                .OrderBy(city => city.Name)
                .ToListAsync();

        }
        
        /// <summary>
        /// this is using IQuery which is appended based on user requirement and execute only when terminal
        /// expression is called like ".ToListAsync()"
        /// Deferred execution
        /// </summary>
        public async Task<IEnumerable<City>> GetAllCitiesUsingSearchAndPaginationAsync(
            string name,
            string searchQuery,
            bool includePoints,
            int pageNumber,
            int pageSize)
        {
            IQueryable<City> queryCollection = CreateFilterAndSearchQuery(name, searchQuery, includePoints);

            return await queryCollection
                .OrderBy(city => city.Name)// always use orderBy in pagination
                .Skip(pageSize * (pageNumber - 1)) // 0th page 10, 1st page 10, 2nd page 10. skip 2 pages result == 10 * (2 - 1), 20 cities skipped
                .Take(pageSize)
                .ToListAsync();

        }
        
        /// <summary>
        /// this is using IQuery which is appended based on user requirement and execute only when terminal
        /// expression is called like ".ToListAsync()"
        /// Deferred execution
        /// </summary>
        public async Task<CityPage> GetAllCitiesWithPaginationMetdadataAsync(
            string name,
            string searchQuery,
            bool includePoints,
            int pageNumber,
            int pageSize)
        {
            IQueryable<City> queryCollection = CreateFilterAndSearchQuery(name, searchQuery, includePoints);

            var countOfItems = await queryCollection.CountAsync();
            var paginationMatadata = new PaginationMetadata(countOfItems, pageSize, pageNumber);

            var cities = await queryCollection
                .OrderBy(city => city.Name)// always use orderBy in pagination
                .Skip(pageSize * (pageNumber - 1)) // 0th page 10, 1st page 10, 2nd page 10. skip 2 pages result == 10 * (2 - 1), 20 cities skipped
                .Take(pageSize)
                .ToListAsync();

            return new CityPage(cities, paginationMatadata);

        }

        /// <summary>
        /// Create filter and search query using IQueryable<City> collections
        /// </summary>
        private IQueryable<City> CreateFilterAndSearchQuery(string name, string searchQuery, bool includePoints)
        {
            var queryCollection = databaseContext.Cities as IQueryable<City>;

            if (includePoints)
            {
                queryCollection = queryCollection
                .Include(city => city.PointOfInterests);

            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                name = name.Trim();
                queryCollection = queryCollection
                .Where(city => city.Name == name);
            }

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                queryCollection = queryCollection
                .Where(city => city.Name.Contains(searchQuery) || city.Description.Contains(searchQuery));
            }

            return queryCollection;
        }
    }
}
