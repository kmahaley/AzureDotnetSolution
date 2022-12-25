using SqlDbApplication.Models.Sql;
using SqlDbApplication.Repositories.Sql.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SqlDbApplication.Repositories.Sql
{
    public class PointOfInterestRepository : IPointOfInterestRepository
    {
        public Task<City> AddPointOfInterestAsync(PointOfInterest pointOfInterest)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<PointOfInterest>> GetAllPointOfInterestsAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<PointOfInterest> GetPointOfInterestByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<PointOfInterest> UpdatePointOfInterestAsync(int id, PointOfInterest pointOfInterest)
        {
            throw new System.NotImplementedException();
        }
    }
}
