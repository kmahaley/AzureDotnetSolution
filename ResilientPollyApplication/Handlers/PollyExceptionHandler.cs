using Polly.CircuitBreaker;
using Polly.Timeout;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ResilientPollyApplication.Handlers
{
    public class PollyExceptionHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                return await base.SendAsync(request, cancellationToken);
            }
            catch (Exception ex) when (ex is TimeoutRejectedException || ex is BrokenCircuitException)
            {
                throw new OperationCanceledException(ex.Message, ex);
            }
        }
    }
}