using AutoMapper;
using Microsoft.Extensions.Logging;
using SqlDbApplication.Models.Dtos;
using SqlDbApplication.Models.Sql;
using SqlDbApplication.Repositories.Sql;
using SqlDbApplication.Repositories.Sql.Interface;
using SqlDbApplication.Services.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SqlDbApplication.Services
{
    public class PointOfInterestService : IPointOfInterestService
    {
        private readonly IPointOfInterestRepository pointRepository;

        private readonly ILogger<PointOfInterestService> logger;

        private readonly IMapper mapper;

        public PointOfInterestService(IPointOfInterestRepository pointRepository, ILogger<PointOfInterestService> logger, IMapper mapper)
        {
            this.pointRepository = pointRepository ?? throw new ArgumentNullException(nameof(pointRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public Task<PointOfInterestDto> AddPointOfInterestAsync(PointOfInterestDto pointOfInterestDto)
        {
            throw new NotImplementedException("No implementation provided in the repository. upcoming API.");
        }

        public async Task<IList<PointOfInterestDto>> GetAllPointOfInterestsAsync()
        {
            var points = await pointRepository.GetAllPointOfInterestsAsync();
            var pointDtos = mapper.Map<IEnumerable<PointOfInterest>, IList<PointOfInterestDto>>(points);
            return pointDtos;
        }

        public async Task<PointOfInterestDto> GetPointOfInterestByIdAsync(int id)
        {
            var point = await pointRepository.GetPointOfInterestByIdAsync(id);
            return mapper.Map<PointOfInterestDto>(point);
        }

        public async Task<PointOfInterestDto> UpdatePointOfInterestAsync(int id, PointOfInterestDto pointOfInterestDto)
        {
            var point = mapper.Map<PointOfInterest>(pointOfInterestDto);
            var updatedPoint =await pointRepository.UpdatePointOfInterestAsync(id, point);
            var updatedPointOfInteresetDto = mapper.Map<PointOfInterestDto>(updatedPoint);
            return updatedPointOfInteresetDto;
        }
    }
}
