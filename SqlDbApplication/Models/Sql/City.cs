using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SqlDbApplication.Models.Sql
{
    public class City
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CityId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        
        public int Population { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }

        public IList<PointOfInterest> PointOfInterests { get; set; } = new List<PointOfInterest>();

        public DateTime CreatedOn { get; set; }
    }
}
