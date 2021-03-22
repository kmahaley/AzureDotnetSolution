using Microsoft.Extensions.DependencyInjection;
using ResilientPollyApplication.Constants;
using ResilientPollyApplication.Handlers;
using ResilientPollyApplication.Services;
using System;
using System.Collections.Generic;
using System.Net;
using UtilityLibrary.PollyProject;

namespace ResilientPollyApplication.Extensions
{
    public static class HttpNamedExtension
    {
        public static IServiceCollection AddHttpNamedBasedDependencies(this IServiceCollection services)
        {
            services.AddSingleton<PollyExceptionHandler>();
            services.AddSingleton<NamedHttpMessageHandler>();
            services.AddSingleton<IHttpService, HttpNamedService>();

            var retryableCode = new List<HttpStatusCode>() { HttpStatusCode.InternalServerError };
            //network failures, 5xx and 408 responses
            services.AddHttpClient("transientpolicy")
                .AddHttpMessageHandler<NamedHttpMessageHandler>()
                .AddPolicyHandler(NamedHttpClientBasedPolicy.CreateTimeoutPolicy(TimeSpan.FromSeconds(5)))
                .AddPolicyHandler(NamedHttpClientBasedPolicy.CreateWaitAndRetryPolicy())
                .AddPolicyHandler(NamedHttpClientBasedPolicy.CreateTimeoutPolicy(TimeSpan.FromSeconds(1)))
                .AddPolicyHandler(NamedHttpClientBasedPolicy.CreateCircuitBreakerPolicy(durationOfTheBreak: TimeSpan.FromSeconds(30)));

            //network failures, 5xx and 408 responses
            services.AddHttpClient(RetryableConstants.PollyBasedNamedHttpClient)
                //.AddHttpMessageHandler<NamedHttpMessageHandler>()
                .AddHttpMessageHandler<PollyExceptionHandler>()
                .AddPolicyHandler(NamedHttpClientBasedPolicy.CreateTimeoutPolicy(TimeSpan.FromSeconds(10)))
                .AddPolicyHandler(NamedHttpClientBasedPolicy.CreateWaitAndRetryPolicy(retryableCode, 3))
                .AddPolicyHandler(NamedHttpClientBasedPolicy.CreateTimeoutPolicy(TimeSpan.FromSeconds(1)));
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