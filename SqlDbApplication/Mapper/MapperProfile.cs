using AutoMapper;
using SqlDbApplication.Models.Dtos;
using SqlDbApplication.Models.Sql;

namespace SqlDbApplication.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CityDto, City>()
                .ReverseMap();

            CreateMap<PointOfInterestDto, PointOfInterest>()
                .ReverseMap();

            CreateMap<CityPageDto, CityPage>()
                .ReverseMap();

        }
    }
}
