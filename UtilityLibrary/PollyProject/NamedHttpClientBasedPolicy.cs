using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using UtilityLibrary.Extensions;

namespace UtilityLibrary.PollyProject
{
    public static class NamedHttpClientBasedPolicy
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
        /// <param name="timeOutDuration">duration of timeout in seconds. Default is 10 seconds.</param>
        /// <returns>Time out policy.</returns>
        public static IAsyncPolicy<HttpResponseMessage> CreateTimeoutPolicy(TimeSpan timeOutDuration)
        {
            timeOutDuration = timeOutDuration == null ? TimeSpan.FromSeconds(10) : timeOutDuration;
            return Policy.TimeoutAsync<HttpResponseMessage>(timeOutDuration, onTimeoutAsync: (context, timeSpan, task) =>
            {
                if (!context.TryGetLogger(out var logger))
                {
                    return Task.CompletedTask;
                }
                logger.LogError("{id} >>>>>>>>>>> Timeout delegate fired after {timeout} seconds", context.CorrelationId, timeSpan.TotalSeconds);
                return Task.CompletedTask;
            });
        }

        public static IAsyncPolicy<HttpResponseMessage> CreateWaitAndRetryPolicy()
        {
            return HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .OrResult(msg => !msg.IsSuccessStatusCode)
                    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(1, retryAttempt)));
        }

        /// <summary>
        /// Creates a wait and retry policy. Optional list custom error status code, user want to retry on.
        /// </summary>
        /// <param name="retryableStatusCode">specify list of custom error code apart from polly defined transient errors.</param>
        /// <param name="numberOfRetries">Number of retries on failure. Default is 3.</param>
        /// <returns>returns asynchronous retry policy.</returns>
        public static IAsyncPolicy<HttpResponseMessage> CreateWaitAndRetryPolicy(IList<HttpStatusCode> retryableStatusCode = null, int numberOfRetries = 3)
        {
            retryableStatusCode ??= new List<HttpStatusCode>();
            return
                HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .Or<TimeoutRejectedException>()
                    .OrResult(msg => retryableStatusCode.Contains(msg.StatusCode))
                    .WaitAndRetryAsync(
                        numberOfRetries,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(1, retryAttempt)),
                        (outcome, timeSpan, retryCount, context) =>
                        {
                            if (!context.TryGetLogger(out var logger))
                            {
                                return;
                            }

                            logger.LogInformation("{id} >>>>>>>>>>> Delaying = {delay}ms, retryAttempt = {retry}.", context.CorrelationId, timeSpan.TotalMilliseconds, retryCount);
                            if (outcome != null && outcome.Result != null)
                            {
                                logger.LogError("{id} >>>>>>>>>>> Previous request error code = {status}.", context.CorrelationId, outcome.Result.StatusCode);
                            }
                        });
        }

        /// <summary>
        /// Create a circuit breaker policy. Optional list custom error status code, user want to break circuit on.
        /// </summary>
        /// <param name="durationOfTheBreak">Time is seconds for which circuit will stay Open(broken). Default is 10 seconds.</param>
        /// <param name="failuresBeforeBreaking">Number of failure before circuit is broken. Default is 10.</param>
        /// <param name="retryableStatusCode">List of error code apart from polly defined transient errors.</param>
        /// <returns>asynchronous circuit breaker policy.</returns>
        public static IAsyncPolicy<HttpResponseMessage> CreateCircuitBreakerPolicy(TimeSpan durationOfTheBreak, int failuresBeforeBreaking = 2, IList<HttpStatusCode> retryableStatusCode = null)
        {
            retryableStatusCode ??= new List<HttpStatusCode>();
            durationOfTheBreak = durationOfTheBreak == null ? TimeSpan.FromSeconds(10) : durationOfTheBreak;
            return HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .OrResult(msg => retryableStatusCode.Contains(msg.StatusCode))
                    .CircuitBreakerAsync(failuresBeforeBreaking, durationOfTheBreak, OnBreak, OnReset);
        }

        /// <summary>
        /// When request get successful response when Circuit state is HalfOpen, OnReset delegate is executed.
        /// </summary>
        /// <param name="context">Polly based context.</param>
        private static void OnReset(Context context)
        {
            if (!context.TryGetLogger(out var logger))
            {
                return;
            }

            logger.LogError("{id} >>>>>>>>>>> Circuit closed, requests will flow normally.", context.CorrelationId);
        }

        /// <summary>
        /// When requests fail beyond defined threshold, circuit is broken/opened, no further request will flow
        /// until circuitBrokenTime is lapsed
        /// </summary>
        /// <param name="result">Result of the last execution.</param>
        /// <param name="circuitBrokenTime">Time for which circuit will remain open</param>
        /// <param name="context">Polly based context</param>
        private static void OnBreak(DelegateResult<HttpResponseMessage> result, TimeSpan circuitBrokenTime, Context context)
        {
            if (!context.TryGetLogger(out var logger))
            {
                return;
            }

            logger.LogError($"id:{context.CorrelationId} >>>>>>>>>>> Circuit broken, requests will not flow. {circuitBrokenTime.TotalMilliseconds}");
        }
    }
}