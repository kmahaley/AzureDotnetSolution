using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ResilientPollyApplication.Handlers
{
    public class TimingHttpMessageHandler : DelegatingHandler
    {

        private readonly ILogger<TimingHttpMessageHandler> _logger;

        public TimingHttpMessageHandler(ILogger<TimingHttpMessageHandler> logger)
        {
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var sw = Stopwatch.StartNew();

            _logger.LogInformation("====================== Starting request");

            var response = await base.SendAsync(request, cancellationToken);

            _logger.LogInformation($"===================== Finished request : {sw.ElapsedMilliseconds}ms");

            return response;
        }
    }
}
