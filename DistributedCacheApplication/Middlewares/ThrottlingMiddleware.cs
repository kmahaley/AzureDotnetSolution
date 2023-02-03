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
            
            // next middleware in pipeline
            await next(httpContext);

            // user request and headers
            logger.LogInformation($"--- after {this.GetType().Name}");
/*
            httpContext.Items.TryGetValue("key", out var messageLeft);
            logger.LogInformation($"items messageLeft key => {messageLeft}");

            Endpoint? endpoint = httpContext.GetEndpoint();
            var throttlingAttributes = endpoint?.Metadata.GetOrderedMetadata<ThrottlingAttribute>() ?? Array.Empty<ThrottlingAttribute>();

            ThrottlingAttribute? att = throttlingAttributes.FirstOrDefault();
            if (att != null)
            {
                logger.LogInformation($"Throttling attribute => {att?.ApiName + att?.ApiAlias + att?.RouteParameter}");

                object routeValue = null;
                var routeData = httpContext.GetRouteData();
                routeData?.Values.TryGetValue(att.RouteParameter, out routeValue);

                if (routeValue is string givenRouteId)
                {
                    logger.LogInformation($"givenRouteId => {givenRouteId}");
                }
            }
            
*/
        }

    }
}
