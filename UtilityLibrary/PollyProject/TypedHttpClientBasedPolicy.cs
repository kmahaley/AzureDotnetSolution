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
using System.Transactions;

namespace UtilityLibrary.PollyProject
{
    public static class TypedHttpClientBasedPolicy
    {
        /// <summary>
        /// Create no operation policy.
        /// This can be used if user is making post call and does not want to have any retry/timeout policies associated with the call.
        /// </summary>
        /// <returns>No operation policy.</returns>
        public static IAsyncPolicy<HttpResponseMessage> CreateNoopPolicy()
        {
            return Policy.NoOpAsync<HttpResponseMessage>();
        }

        /// <summary>
        /// Create time out policy. This timeout duration will supercede http client time out duration.
        /// </summary>
        /// <param name="timeOutDuration">duration of timeout in seconds. Default is 2 seconds.</param>
        /// <returns>Time out policy.</returns>
        public static IAsyncPolicy<HttpResponseMessage> CreateTimeoutPolicy(int timeOutDuration = 5)
        {
            return Policy.TimeoutAsync<HttpResponseMessage>(timeOutDuration);
        }

        public static IAsyncPolicy<HttpResponseMessage> CreateWaitAndRetryPolicy() {

            return HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .OrResult(msg => !msg.IsSuccessStatusCode)
                    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(1, retryAttempt)));

        }

        /// <summary>
        /// Creates a wait and retry policy. Optional list custom error status code, user want to retry on.
        /// </summary>
        /// <param name="retryableStatusCode">specify list of custom error code apart from polly defined transient errors.</param>
        /// /// <param name="numberOfRetries">Number of retries on failure.</param>
        /// <returns>function that takes service provider and returns asynchronous policy.</returns>
        public static Func<IServiceProvider, HttpRequestMessage, IAsyncPolicy<HttpResponseMessage>> CreateWaitAndRetryPolicy<T>(List<int> retryableStatusCode = null, int numberOfRetries = 3)
        {
            retryableStatusCode ??= new List<int>();
            return (services, request) =>
                HttpPolicyExtensions
                    .HandleTransientHttpError()
                    //.Or<TimeoutRejectedException>()
                    .OrResult(msg => {
                        var responseCode = (int)msg.StatusCode;
                        return retryableStatusCode.Contains(responseCode);
                    })
                    .WaitAndRetryAsync(numberOfRetries, 
                                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                                    (outcome, timeSpan, retryCount, context) => {

                                        var logger = services.GetRequiredService<ILogger<T>>();
                                        logger.LogInformation("Delaying request {request} for {delay}ms, then making retry {retry}.", request.RequestUri, timeSpan.TotalMilliseconds, retryCount);
                                        if(outcome != null && outcome.Result != null)
                                        {
                                            logger.LogError("Error with code {status} received on request {request}.", outcome.Result.StatusCode, request.RequestUri);
                                        }
                                    });

        }

        /// <summary>
        /// Create a circuit breaker policy. Optional list custom error status code, user want to break circuit on.
        /// </summary>
        /// <param name="retryableStatusCode">List of error code apart from polly defined transient errors.</param>
        /// <param name="failuresBeforeBreaking">Number of failure before circuit is broken.</param>
        /// <param name="durationOfTheBreak">Time is seconds for which circuit will stay Open(broken).</param>
        /// <returns>asynchronous circuit breaker policy.</returns>
        public static IAsyncPolicy<HttpResponseMessage> CreateCircuitBreakerPolicy(List<int> retryableStatusCode = null, int failuresBeforeBreaking = 10, int durationOfTheBreak = 10)
        {
            retryableStatusCode ??= new List<int>();
            return HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .OrResult(msg => {
                        var responseCode = (int)msg.StatusCode;
                        return retryableStatusCode.Contains(responseCode);
                    })
                    .CircuitBreakerAsync(failuresBeforeBreaking, TimeSpan.FromSeconds(durationOfTheBreak));

        }
    }
}
