using Polly;
using Polly.Retry;
using ResilientPollyApplication.Constants;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace ResilientPollyApplication.Polly
{
    public static class PolicyUtilities
    {
        public static RetryPolicy<HttpResponseMessage> CreateWaitAndRetryPolicyWithListOfStatusCode()
        {
            return Policy
                    .Handle<HttpRequestException>()
                    .OrResult<HttpResponseMessage>(r => RetryableConstants.httpStatusCodesWorthRetrying.Contains(r.StatusCode))
                    .WaitAndRetry(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        public static RetryPolicy<HttpResponseMessage> CreateWaitAndRetryPolicyOnResponse()
        {
            return Policy
                    .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                    .WaitAndRetry(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        public static RetryPolicy CreateWaitAndRetryPolicyOnException()
        {
            return Policy
                    .Handle<Exception>()
                    .WaitAndRetry(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
    }
}
