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
        public static void AddHttpTypedBasedDependencies(this ServiceCollection services)
        {
            /*
            services.AddHttpClient<IHttpService, HttpTypedService>()
                .AddPolicyHandler(HttpPolicyUtils.WaitAndRetryPolicy<HttpClientTypedService>())
                .AddPolicyHandler(HttpPolicyUtils.GetCircuitBreakerPolicy());
            */
        }
    }
}
