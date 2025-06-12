using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace SqlDbApplication.Models.Sql
{
    public class VnetPrivateEndpointTargetResourceMetadata
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [ForeignKey(nameof(VnetPrivateEndpointMetadata))]
        public Guid PrivateEndpointId { get; set; }

        public VnetPrivateEndpointMetadata VnetPrivateEndpointMetadata { get; set; }

        public string GroupId { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
