using DistributedCacheApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using Microsoft.VisualBasic;
using System.Diagnostics;
using System.Net;

namespace DistributedCacheApplication.Filters
{
    public class HttpETagFilter : IAsyncActionFilter
    {
        private readonly string filterName = nameof(HttpETagFilter);

        private readonly ILogger<HttpETagFilter> logger;

        public HttpETagFilter(ILogger<HttpETagFilter> logger)
        {
            this.logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext executingContext, ActionExecutionDelegate next)
        {
            logger.LogInformation($"--- before action filter. {filterName}");
            var executedContext = await next();
            logger.LogInformation($"--- after action filter. {filterName}");

            var request = executedContext.HttpContext.Request;
            var response = executedContext.HttpContext.Response;

            //Computing ETags for Response Caching on GET requests
            if (request.Method == HttpMethod.Get.Method
                && response.StatusCode == (int)HttpStatusCode.OK)
            {
                logger.LogInformation("Validate Etag");
                ValidateETagForResponseCaching(executedContext);
            }
        }

        private void ValidateETagForResponseCaching(ActionExecutedContext executedContext)
        {
            if (executedContext.Result == null)
            {
                return;
            }

            var httpContext = executedContext.HttpContext;
            var request = httpContext.Request;
            var response = httpContext.Response;
            var result = (Product)(executedContext.Result as ObjectResult).Value;
            // generates ETag from the entire response Content
            var etag = GenerateEtagFromResponseBodyWithHash(result);
            if (request.Headers.ContainsKey(HeaderNames.IfNoneMatch))
            {
                // fetch etag from the incoming request header
                var incomingEtag = request.Headers[HeaderNames.IfNoneMatch].ToString();

                // if both the etags are equal
                // raise a 304 Not Modified Response
                if (incomingEtag.Equals(etag))
                {
                    executedContext.Result = new StatusCodeResult((int)HttpStatusCode.NotModified);
                }
            }

            // add ETag response header 
            response.Headers.Add(HeaderNames.ETag, new[] { etag });
        }

        private string GenerateEtagFromResponseBodyWithHash(Product? serverProduct)
        {
            return "apple";
        }
    }
}
