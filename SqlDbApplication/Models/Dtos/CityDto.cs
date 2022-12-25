using SqlDbApplication.Models.Sql;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SqlDbApplication.Models.Dtos
{
    public class CityDto
    {
        public int CityId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public int Population { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }

        public IList<PointOfInterestDto> PointOfInterests { get; set; } = new List<PointOfInterestDto>();
    }
}
