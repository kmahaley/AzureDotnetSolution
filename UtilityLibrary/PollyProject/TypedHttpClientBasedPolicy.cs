using Polly;
using Polly.Extensions.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace UtilityLibrary.PollyProject
{
    public static class TypedHttpClientBasedPolicy
    {
        public static IAsyncPolicy<HttpResponseMessage> CreateNoopPolicy()
        {
            return Policy.NoOpAsync<HttpResponseMessage>();
        }

        public static IAsyncPolicy<HttpResponseMessage> CreateTimeoutPolicy()
        {
            return Policy.TimeoutAsync<HttpResponseMessage>(30);
        }

        public static IAsyncPolicy<HttpResponseMessage> CreateWaitAndRetryPolicy() {

            return HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .OrResult(msg => !msg.IsSuccessStatusCode)
                    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(1, retryAttempt)));

        }

        public static IAsyncPolicy<HttpResponseMessage> CreateWaitAndRetryPolicy(List<int> retryableStatusCode)
        {

            return HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .OrResult(msg => {
                        var responseCode = (int)msg.StatusCode;
                        return retryableStatusCode.Contains(responseCode);
                    })
                    .WaitAndRetryAsync(3, 
                                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(1, retryAttempt)),
                                    (outcome, timeSpan, retryCount, context) => {
                                        if(outcome != null && outcome.Exception != null)
                                        {
                                            Console.WriteLine($"==== {outcome.Exception.Message}, {timeSpan}, {retryCount}");
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
