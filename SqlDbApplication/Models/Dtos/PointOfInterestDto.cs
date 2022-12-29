using SqlDbApplication.Models.Sql;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SqlDbApplication.Models.Dtos
{
    /// <summary>
    /// Tourist locations in the city
    /// </summary>
    public class PointOfInterestDto
    {
        /// <summary>
        /// unique identifier of the tourist location
        /// </summary>
        public int PointOfInterestId { get; set; }

        /// <summary>
        /// Name of the tourist location
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Location of the torurist location
        /// </summary>
        [MaxLength(200)]
        public string? Description { get; set; }
    }
}
