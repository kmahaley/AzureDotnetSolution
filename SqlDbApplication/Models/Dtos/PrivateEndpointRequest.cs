using Microsoft.AspNetCore.Mvc;

using SqlDbApplication.Models.Sql;

namespace SqlDbApplication.Models.Dtos
{
    public class PrivateEndpointRequest
    {
        public VnetPrivateEndpointMetadata PrivateEndpointMetadata { get; set; }

        public VnetPrivateEndpointReferenceMetadata ReferenceMetadata { get; set; }

        public VnetPrivateEndpointTargetResourceMetadata TargetResourceMetadata { get; set; }
    }
}
