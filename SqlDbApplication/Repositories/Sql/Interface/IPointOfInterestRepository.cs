using SqlDbApplication.Models.Sql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SqlDbApplication.Repositories.Sql.Interface
{
    public interface IPointOfInterestRepository
    {
        Task<City> AddPointOfInterestAsync(PointOfInterest pointOfInterest);
        Task<IEnumerable<PointOfInterest>> GetAllPointOfInterestsAsync();
        Task<PointOfInterest> GetPointOfInterestByIdAsync(int id);
        Task<PointOfInterest> UpdatePointOfInterestAsync(int id, PointOfInterest pointOfInterest);
    }
}
 