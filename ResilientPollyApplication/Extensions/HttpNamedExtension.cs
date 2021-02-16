using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResilientPollyApplication.Extensions
{
    public static class HttpNamedExtension
    {
        public static void AddHttpNamedBasedDependencies(this ServiceCollection services)
        {
            /*
            //network failures, 5xx and 408 responses
            services.AddHttpClient("transientpolicy")
                .AddHttpMessageHandler<TimingHandler>()
                //.AddTransientHttpErrorPolicy(HttpPolicyUtils.GetRetryPolicyWithTimeBetweenCalls())
                //.AddTransientHttpErrorPolicy(HttpPolicyUtils.GetRetryPolicy())
                //.AddTransientHttpErrorPolicy(HttpPolicyUtils.GetCircuitBreakerPolicy())
                .AddTransientHttpErrorPolicy(HttpPolicyUtils.GetRetryPolicyWithTimeBetweenCalls());


            //AddPolicyHandler: you define what and how to handle
            services.AddHttpClient("conditionalpolicy")
                .AddHttpMessageHandler<TimingHandler>()
                .AddPolicyHandler(HttpPolicyUtils.PolicyWithExceptionAndRetry())
                .AddPolicyHandler(HttpPolicyUtils.GetCircuitBreakerPolicyFromExtension())
                .AddPolicyHandler(HttpPolicyUtils.TimeoutPolicy());


            services.AddHttpClient("selectPolicy")
                .AddHttpMessageHandler<TimingHandler>()
                .AddPolicyHandler(request => request.Method == HttpMethod.Get ? HttpPolicyUtils.PolicyWithExceptionAndRetry() : HttpPolicyUtils.NoOperationPolicy());

            */
        }
    }
}
