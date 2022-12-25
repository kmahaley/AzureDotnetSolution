using SqlDbApplication.Models.Sql;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SqlDbApplication.Models.Dtos
{
    public class PointOfInterestDto
    {
        public int PointOfInterestId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }
    }
}
