using Microsoft.Extensions.DependencyInjection;
using ResilientPollyApplication.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResilientPollyApplication.Extensions
{
    public static class HttpTypedExtension
    {
        public static IServiceCollection AddHttpTypedBasedDependencies(this IServiceCollection services)
        {
            /*
            services.AddHttpClient<IHttpService, HttpTypedService>()
                .AddHttpMessageHandler()
                .AddPolicyHandler(HttpPolicyUtils.WaitAndRetryPolicy<HttpClientTypedService>())
                .AddPolicyHandler(HttpPolicyUtils.GetCircuitBreakerPolicy());
            */
            return services;
        }
    }
}
