using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
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
            return Policy.TimeoutAsync<HttpResponseMessage>(timeOutDuration);
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
                    .OrResult(msg => retryableStatusCode.Contains(msg.StatusCode))
                    .WaitAndRetryAsync(
                        numberOfRetries,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(1, retryAttempt)),
                        (outcome, timeSpan, retryCount, context) =>
                        {
                            if(!context.TryGetLogger(out var logger))
                            {
                                return;
                            }
                                
                            logger.LogInformation(" >>>>>>>>>>> Delaying = {delay}ms, retryAttempt = {retry}.", timeSpan.TotalMilliseconds, retryCount);
                            if(outcome != null && outcome.Result != null)
                            {
                                logger.LogError(" >>>>>>>>>>> Previous request error code = {status}.", outcome.Result.StatusCode);
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

        

        private static void OnReset(Context context)
        {
            if(!context.TryGetLogger(out var logger))
            {
                return;
            }
            logger.LogError(" >>>>>>>>>>> Circuit closed, requests will flow normally.");
        }

        private static void OnBreak(DelegateResult<HttpResponseMessage> result, TimeSpan ts, Context context)
        {
            if(!context.TryGetLogger(out var logger))
            {
                return;
            }
            logger.LogError($" >>>>>>>>>>> Circuit brocken, requests will not flow. {ts.TotalMilliseconds}");
        }
    }
}
