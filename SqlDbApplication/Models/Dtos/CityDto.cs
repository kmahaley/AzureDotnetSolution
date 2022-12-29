using SqlDbApplication.Models.Sql;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SqlDbApplication.Models.Dtos
{
    /// <summary>
    /// City data
    /// </summary>
    public class CityDto
    {
        /// <summary>
        /// City identifier
        /// </summary>
        public int CityId { get; set; }

        /// <summary>
        /// Name of the city
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Population of the city
        /// </summary>
        public int Population { get; set; }

        /// <summary>
        /// Description of the city
        /// </summary>
        [MaxLength(200)]
        public string? Description { get; set; }

        /// <summary>
        /// List of interesting location inside city. Tourist locations
        /// </summary>
        public IList<PointOfInterestDto> PointOfInterests { get; set; } = new List<PointOfInterestDto>();
    }
}
