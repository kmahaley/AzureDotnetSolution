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
using System.Threading;
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

        public async Task<City> AddCityAsync(City city, CancellationToken cancellationToken)
        {
            var savedCity = await databaseContext.Cities.AddAsync(city, cancellationToken);
            await databaseContext.SaveChangesAsync(cancellationToken);
            return savedCity.Entity;
        }

        public async Task<City> DeleteCityByIdAsync(int id, CancellationToken cancellationToken)
        {
            var existingCity = await GetCityAsync(id, cancellationToken);

            databaseContext.Cities.Remove(existingCity);
            await databaseContext.SaveChangesAsync(cancellationToken);
            return existingCity;
        }

        public async Task<IEnumerable<City>> GetAllCitiesAsync(
            bool includePoints,
            CancellationToken cancellationToken)
        {
            if (includePoints)
            {

                return await databaseContext
                    .Cities
                    .Include(city => city.PointOfInterests)
                    .ToListAsync(cancellationToken);
            }

            return await databaseContext.Cities.ToListAsync(cancellationToken);
        }

        public async Task<City> GetCityByIdAsync(int id, bool includePoints, CancellationToken cancellationToken)
        {
            if(!includePoints)
            {
                return await GetCityAsync(id, cancellationToken);
            }

            var city = await databaseContext
                    .Cities
                    .Where(c => c.CityId == id)
                    .Include(c => c.PointOfInterests)
                    .FirstOrDefaultAsync(cancellationToken);

            if (city == null)
            {
                throw new SqlDbApplicationException(
                    $"City is not present with id {id}",
                    ErrorCode.IncorrectEntityIdProvided);
            }

            return city;
        }

        public async Task<City> UpdateCityAsync(
            int id,
            City city,
            CancellationToken cancellationToken)
        {
            var existingCity = await GetCityAsync(id, cancellationToken);
            existingCity.Name = city.Name;
            existingCity.Population = city.Population;
            existingCity.Description = city.Description;
            await databaseContext.SaveChangesAsync(cancellationToken);
            return city;
        }

        public async Task<City> GetCityAsync(int id, CancellationToken cancellationToken)
        {
            var city = await databaseContext
                .Cities
                .Where(city => city.CityId == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (city == null)
            {
                throw new SqlDbApplicationException(
                    $"City is not present with id {id}",
                    ErrorCode.IncorrectEntityIdProvided);
            }
            return city;
        }

        public async Task<IEnumerable<City>> GetAllCitiesFilteredUsingNameAsync(
            string name,
            bool includePoints,
            CancellationToken cancellationToken) 
        {
            if (includePoints)
            {
                return await databaseContext
                .Cities
                .Include(city => city.PointOfInterests)
                .Where(city => city.Name == name)
                .ToListAsync(cancellationToken);
            }

            return await databaseContext
                .Cities
                .Where(city => city.Name == name)
                .ToListAsync(cancellationToken);

        }

        /// <summary>
        /// this is using IQuery which is appended based on user requirement and execute only when terminal
        /// expression is called like ".ToListAsync()"
        /// Deferred execution
        /// </summary>
        public async Task<IEnumerable<City>> GetAllCitiesUsingSearchAsync(
            string name,
            string searchQuery,
            bool includePoints,
            CancellationToken cancellationToken)
        {

            var queryCollection = CreateFilterAndSearchQuery(name, searchQuery, includePoints);

            return await queryCollection
                .OrderBy(city => city.Name)
                .ToListAsync(cancellationToken);

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
            int pageSize,
            CancellationToken cancellationToken)
        {
            IQueryable<City> queryCollection = CreateFilterAndSearchQuery(name, searchQuery, includePoints);

            return await queryCollection
                .OrderBy(city => city.Name)// always use orderBy in pagination
                .Skip(pageSize * (pageNumber - 1)) // 0th page 10, 1st page 10, 2nd page 10. skip 2 pages result == 10 * (2 - 1), 20 cities skipped
                .Take(pageSize)
                .ToListAsync(cancellationToken);

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
            int pageSize,
            CancellationToken cancellationToken)
        {
            IQueryable<City> queryCollection = CreateFilterAndSearchQuery(name, searchQuery, includePoints);

            var countOfItems = await queryCollection.CountAsync(cancellationToken);
            var paginationMatadata = new PaginationMetadata(countOfItems, pageSize, pageNumber);

            var cities = await queryCollection
                .OrderBy(city => city.Name)// always use orderBy in pagination
                .Skip(pageSize * (pageNumber - 1)) // 0th page 10, 1st page 10, 2nd page 10. skip 2 pages result == 10 * (2 - 1), 20 cities skipped
                .Take(pageSize)
                .ToListAsync(cancellationToken);

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
