using System.Collections.Generic;

namespace SqlDbApplication.Models.Sql
{
    public class CityPage
    {
        public IEnumerable<City> Cities { get; set; }
        public PaginationMetadata PaginationMetadata { get; set; }

        public CityPage(IEnumerable<City> cities, PaginationMetadata paginationMetadata)
        {
            Cities = cities;
            PaginationMetadata = paginationMetadata;
        }
    }
}
