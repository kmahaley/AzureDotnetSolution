using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoreWebApplication
{
    public class RequestTimeoutPolicy : IResilientPolicy
    {
        private readonly ILogger logger;
        private readonly TimeSpan requestTimeout;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestTimeoutPolicy"/> class.
        /// </summary>
        /// <param name="logger">Instance for logging the errors.</param>
        /// <param name="serviceConfig">service configurations.</param>
        /// <param name="metrics">perfomance metrics.</param>
        public RequestTimeoutPolicy(ILogger logger)
        {
            this.logger = logger;

            requestTimeout = TimeSpan.FromSeconds(15);

            Policy = CreateTimeoutPolicy(requestTimeout, logger);
        }

        public IAsyncPolicy<HttpResponseMessage> Policy { get; }

        private static IAsyncPolicy<HttpResponseMessage> CreateTimeoutPolicy(TimeSpan timeOutDuration, ILogger logger)
        {
            logger.LogInformation("---------------------------- create policy called");
            return Polly.Policy.TimeoutAsync<HttpResponseMessage>(timeOutDuration, onTimeoutAsync: (context, timeSpan, task) =>
            {
                logger.LogError($"CorrelationId:{context.CorrelationId}, OperationName:{context.OperationKey} >>>>>>>>>>> Timeout delegate fired after {timeSpan.TotalSeconds} seconds");

                return Task.CompletedTask;
            });
        }
    }
}