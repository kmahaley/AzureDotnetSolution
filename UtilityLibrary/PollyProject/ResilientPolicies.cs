using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace UtilityLibrary.PollyProject
{
    public static class ResilientPolicies
    {
        // Create Polly policy to retry http calls which failed due to transient errors.
        // Call is retried 3 times with delay of 1, 5, 10 seconds
        public static Func<IServiceProvider, HttpRequestMessage, IAsyncPolicy<HttpResponseMessage>> WaitAndRetryPolicy<T>()
        {
            return (services, request) => HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(response => response.StatusCode != HttpStatusCode.OK)
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (outcome, timespan, retryAttempt, context) =>
                {
                    var logger = services.GetRequiredService<ILogger<T>>();
                    logger.LogError($"-----------Call failed {request.RequestUri}. Retrying with delay of: {timespan.TotalMilliseconds}ms, retry attempt: {retryAttempt}.");

                    if(outcome != null && outcome.Result != null)
                    {
                        var result = outcome.Result;
                        logger.LogError($"----------- Failed call status: {(int)result.StatusCode}, reason {result.ReasonPhrase}");
                    }
                });
        }

        /// <summary>
        /// what errors and how to handle is defined
        /// Circuit breaker pattern when transient error occurs
        /// </summary>
        public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(10, TimeSpan.FromSeconds(30));
        }

        // Each http call should not last for more than 10 seconds
        public static AsyncTimeoutPolicy<HttpResponseMessage> GetTimeoutPolicy()
        {
            return Policy.TimeoutAsync<HttpResponseMessage>(10);
        }
    }
}
