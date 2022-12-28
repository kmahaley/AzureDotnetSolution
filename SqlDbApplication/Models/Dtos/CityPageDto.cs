using SqlDbApplication.Models.Sql;
using System.Collections.Generic;

namespace SqlDbApplication.Models.Dtos
{
    public class CityPageDto
    {
        public IEnumerable<CityDto> Cities { get; set; }
        public PaginationMetadata PaginationMetadata { get; set; }

        public CityPageDto(IEnumerable<CityDto> cities, PaginationMetadata paginationMetadata)
        {
            Cities = cities;
            PaginationMetadata = paginationMetadata;
        }
    }
}
