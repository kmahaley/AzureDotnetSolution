using Microsoft.Extensions.DependencyInjection;

namespace ResilientPollyApplication.Extensions
{
    public static class HttpNamedExtension
    {
        public static IServiceCollection AddHttpNamedBasedDependencies(this IServiceCollection services)
        {
            /*
            //network failures, 5xx and 408 responses
            services.AddHttpClient("transientpolicy")
                .AddHttpMessageHandler<TimingHttpMessageHandler>()
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
            return services;
        }
    }
}
