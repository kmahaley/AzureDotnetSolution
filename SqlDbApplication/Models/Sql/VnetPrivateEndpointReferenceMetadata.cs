using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.ComponentModel.DataAnnotations;

namespace SqlDbApplication.Models.Sql
{
    public class VnetPrivateEndpointReferenceMetadata
    {
        public Guid Id { get; set; }

        [ForeignKey(nameof(VnetPrivateEndpointMetadata))]
        public Guid PrivateEndpointId { get; set; }

        public VnetPrivateEndpointMetadata VnetPrivateEndpointMetadata { get; set; }

        [Required]
        [MaxLength(50)]
        public string ReferenceName { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
