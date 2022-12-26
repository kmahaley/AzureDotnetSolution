using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SqlDbApplication.Exceptions;
using SqlDbApplication.Models.Sql;
using SqlDbApplication.Repositories.Sql.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SqlDbApplication.Repositories.Sql
{
    public class PointOfInterestRepository : IPointOfInterestRepository
    {
        private readonly SqlDatabaseContext databaseContext;

        private readonly ILogger<PointOfInterestRepository> logger;

        public PointOfInterestRepository(SqlDatabaseContext databaseContext, ILogger<PointOfInterestRepository> logger)
        {
            this.databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public Task<City> AddPointOfInterestAsync(PointOfInterest pointOfInterest)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<PointOfInterest>> GetAllPointOfInterestsAsync()
        {
            var list = await databaseContext.PointOfInterests.ToListAsync();
            return list;
        }

        public async Task<PointOfInterest> GetPointOfInterestByIdAsync(int id)
        {
            var pointOfInterest = await databaseContext.PointOfInterests.FindAsync(id);
            if (pointOfInterest == null)
            {
                throw new SqlDbApplicationException(
                    $"Point of interest is not present with id {id}",
                    ErrorCode.IncorrectEntityIdProvided);
            }
            return pointOfInterest;
        }

        public async Task<PointOfInterest> UpdatePointOfInterestAsync(
            int id,
            PointOfInterest pointOfInterest)
        {
            var existingPoint = await GetPointOfInterestByIdAsync(id);
            existingPoint.Name = pointOfInterest.Name;
            existingPoint.Description = pointOfInterest.Description;
            await databaseContext.SaveChangesAsync();
            return existingPoint;
        }


    }
}
