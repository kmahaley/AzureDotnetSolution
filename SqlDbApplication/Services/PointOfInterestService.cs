using SqlDbApplication.Models.Dtos;
using SqlDbApplication.Services.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SqlDbApplication.Services
{
    public class PointOfInterestService : IPointOfInterestService
    {
        public Task<PointOfInterestDto> AddPointOfInterestAsync(PointOfInterestDto pointOfInterest)
        {
            throw new System.NotImplementedException();
        }

        public Task<IList<PointOfInterestDto>> GetAllPointOfInterestsAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<PointOfInterestDto> GetPointOfInterestByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<PointOfInterestDto> UpdatePointOfInterestAsync(int id, PointOfInterestDto pointOfInterest)
        {
            throw new System.NotImplementedException();
        }
    }
}
