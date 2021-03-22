﻿using Microsoft.Extensions.DependencyInjection;
using ResilientPollyApplication.Services;
using System.Collections.Generic;
using UtilityLibrary.PollyProject;

namespace ResilientPollyApplication.Extensions
{
    public static class HttpTypedExtension
    {
        public static IServiceCollection AddHttpTypedBasedDependencies(this IServiceCollection services)
        {
            var retryableCodes = new List<int>() { 422, 423 };

            services.AddHttpClient<IHttpService, HttpTypedService>()
                //.AddHttpMessageHandler<TimingHttpMessageHandler>()
                .AddPolicyHandler(TypedHttpClientBasedPolicy.CreateTimeoutPolicy())
                .AddPolicyHandler(TypedHttpClientBasedPolicy.CreateWaitAndRetryPolicy<HttpTypedService>(retryableCodes))
                .AddPolicyHandler(TypedHttpClientBasedPolicy.CreateCircuitBreakerPolicy(retryableCodes));

            return services;
        }
    }
}