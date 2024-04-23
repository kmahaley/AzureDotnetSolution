using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace CoreWebApplication.Middlewares
{
    public class InspectHttpRequestMiddleware : IMiddleware
    {
        private readonly ILogger<InspectHttpRequestMiddleware> logger;

        public InspectHttpRequestMiddleware(ILogger<InspectHttpRequestMiddleware> logger)
        {
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            logger.LogInformation("added middleware");
            var endpoint = context.GetEndpoint();
            var method = context.Request.Method;
            var path = context.Request.Path;

            if (string.Equals(nameof(HttpMethod.Get), method, StringComparison.OrdinalIgnoreCase)
                && path.Value.Equals("/swagger/v1/swagger.json", StringComparison.OrdinalIgnoreCase))
            {
                logger.LogInformation($"{path}");
            }

            // next middleware in pipeline
            await next(context);

            // user request and headers
            logger.LogInformation($"--- after {this.GetType().Name}");
        }
    }
}
