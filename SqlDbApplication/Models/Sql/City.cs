using System.ComponentModel.DataAnnotations;

namespace SqlDbApplication.Models.Sql
{
    public class City
    {
        [Key]
        public int CityId { get; set; }

        [Required]
        public string Name { get; set; }
        
        public int Population { get; set; }

        public string? Description { get; set; }
    }
}
