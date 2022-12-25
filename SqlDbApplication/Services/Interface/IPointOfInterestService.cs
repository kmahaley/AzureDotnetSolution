using SqlDbApplication.Models.Dtos;
using SqlDbApplication.Models.Sql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SqlDbApplication.Services.Interface
{
    public interface IPointOfInterestService
    {
        Task<PointOfInterestDto> AddPointOfInterestAsync(PointOfInterestDto pointOfInterest);
        Task<IList<PointOfInterestDto>> GetAllPointOfInterestsAsync();
        Task<PointOfInterestDto> GetPointOfInterestByIdAsync(int id);
        Task<PointOfInterestDto> UpdatePointOfInterestAsync(int id, PointOfInterestDto pointOfInterest);
    }
}
