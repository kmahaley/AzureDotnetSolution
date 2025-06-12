using System;
using System.Threading;
using System.Threading.Tasks;

using SqlDbApplication.Models.Dtos;

namespace SqlDbApplication.Repositories.Sql.Interface
{
    public interface IPrivateEndpointRepository
    {
        Task AddPrivateEndpointAsync(PrivateEndpointRequest request, CancellationToken token);

        Task<PrivateEndpointRequest> GetPrivateEndpointAsync(Guid peId, CancellationToken token);

        Task DeletePrivateEndpointAsync(Guid peId, CancellationToken token);
    }
}
