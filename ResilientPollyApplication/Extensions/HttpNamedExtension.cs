using Microsoft.Extensions.DependencyInjection;
using ResilientPollyApplication.Handlers;
using ResilientPollyApplication.Services;
using System.Collections.Generic;
using UtilityLibrary.PollyProject;

namespace ResilientPollyApplication.Extensions
{
    public static class HttpNamedExtension
    {
        public static IServiceCollection AddHttpNamedBasedDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IHttpService, HttpNamedService>();

            //network failures, 5xx and 408 responses
            services.AddHttpClient("transientpolicy")
                //.AddHttpMessageHandler<NamedHttpMessageHandler>()
                .AddPolicyHandler(NamedHttpClientBasedPolicy.CreateTimeoutPolicy())
                .AddPolicyHandler(NamedHttpClientBasedPolicy.CreateWaitAndRetryPolicy<HttpNamedService>(new List<int>()))
                .AddPolicyHandler(NamedHttpClientBasedPolicy.CreateCircuitBreakerPolicy(new List<int>()));
            
            /*
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
            
            return services;
        }
    }
}
