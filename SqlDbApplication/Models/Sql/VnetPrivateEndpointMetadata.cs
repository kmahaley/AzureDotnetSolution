using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace SqlDbApplication.Models.Sql
{
    public class VnetPrivateEndpointMetadata
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public string ClientId { get; set; }

        public string Status { get; set; }

        public DateTime CreatedOn { get; set; }

        [ConcurrencyCheck]
        public DateTime LastUpdatedOn { get; set; }

        public string ErrorMsg { get; set; }
    }
}
