using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace UtilityLibrary.PollyProject
{
    public static class NamedHttpClientBasedPolicy
    {
        public static IAsyncPolicy<HttpResponseMessage> CreateNoopPolicy()
        {
            return Policy.NoOpAsync<HttpResponseMessage>();
        }

        public static IAsyncPolicy<HttpResponseMessage> CreateTimeoutPolicy()
        {
            return Policy.TimeoutAsync<HttpResponseMessage>(30);
        }

        public static IAsyncPolicy<HttpResponseMessage> CreateWaitAndRetryPolicy()
        {

            return HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .OrResult(msg => !msg.IsSuccessStatusCode)
                    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(1, retryAttempt)));

        }

        public static Func<IServiceProvider, HttpRequestMessage, IAsyncPolicy<HttpResponseMessage>> CreateWaitAndRetryPolicy<T>(List<int> retryableStatusCode)
        {
            return (services, request) =>
                HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .OrResult(msg => {
                        var responseCode = (int)msg.StatusCode;
                        return retryableStatusCode.Contains(responseCode);
                    })
                    .WaitAndRetryAsync(3,
                                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(1, retryAttempt)),
                                    (outcome, timeSpan, retryCount, context) => {

                                        var logger = services.GetRequiredService<ILogger<T>>();

                                        if(outcome != null && outcome.Exception != null)
                                        {
                                            logger.LogInformation("###### {exception}. Delaying for {delay}ms, then making retry {retry}.", outcome.Result.StatusCode, timeSpan.TotalMilliseconds, retryCount);
                                        }

                                    });

        }

        public static IAsyncPolicy<HttpResponseMessage> CreateCircuitBreakerPolicy(List<int> retryableStatusCode)
        {

            return HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .OrResult(msg => {
                        var responseCode = (int)msg.StatusCode;
                        return retryableStatusCode.Contains(responseCode);
                    })
                    .CircuitBreakerAsync(10, TimeSpan.FromSeconds(10));

        }
    }
}
