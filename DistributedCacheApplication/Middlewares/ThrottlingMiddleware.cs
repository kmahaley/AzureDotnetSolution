using DistributedCacheApplication.Attributes;
using Microsoft.Net.Http.Headers;
using System.Net;

namespace DistributedCacheApplication.Middlewares
{
    public class ThrottlingMiddleware : IMiddleware
    {
        private readonly ILogger<ThrottlingMiddleware> logger;

        public ThrottlingMiddleware(ILogger<ThrottlingMiddleware> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {
            logger.LogInformation($"--- before {this.GetType().Name} move to next middleware");
            await next(httpContext);

            // user request and headers
            logger.LogInformation($"--- after {this.GetType().Name}");
            //var routeValues = httpContext.GetRouteData().Values;

            //routeValues.TryGetValue("id", out var givenRouteId);
            //logger.LogInformation($"givenRouteId => {givenRouteId}");

            Endpoint? endpoint = httpContext.GetEndpoint();
            var throttlingAttributes = endpoint?.Metadata.GetOrderedMetadata<ThrottlingAttribute>() ?? Array.Empty<ThrottlingAttribute>();

            ThrottlingAttribute? att = throttlingAttributes.FirstOrDefault();
            logger.LogInformation($"Throttling attribute => {att?.ApiName + att?.ApiAlias + att?.RouteParameter}");

            httpContext.Items.TryGetValue("key", out var messageLeft);
            logger.LogInformation($"items messageLeft => {messageLeft}");
            


            var userRequest = httpContext.Request;
            var serverResponse = httpContext.Response;


            // next middleware in pipeline
        }

    }
}
