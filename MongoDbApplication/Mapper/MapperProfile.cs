using AutoMapper;
using MongoDbApplication.Dtos;
using MongoDbApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDbApplication.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<ItemDto, Item>()
                .ReverseMap();
        }
    }
}
