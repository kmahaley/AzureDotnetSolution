using DistributedCacheApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net;

namespace DistributedCacheApplication.Filters
{
    public class HttpETagFilter : IAsyncActionFilter
    {
        private readonly string filterName = nameof(HttpETagFilter);

        private JTokenEqualityComparer equalityComparer = new JTokenEqualityComparer();

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
                && IsSuccessStatusCode(response.StatusCode)
                && (executedContext.Result is ObjectResult))
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
            var generatedEtag = string.Empty;

            if (executedContext.Result is ObjectResult objectResultFromResponse)
            {
                object objectFromControllerApi = objectResultFromResponse.Value;
                // generates ETag from the entire response Content
                generatedEtag = GenerateEtagFromResponseBodyWithHash(objectFromControllerApi);
            }
            else
            {
                return;
            } 
            
            if (request.Headers.ContainsKey(HeaderNames.IfNoneMatch))
            {
                // fetch etag from the incoming request header
                var incomingEtag = request.Headers[HeaderNames.IfNoneMatch].ToString();

                // if both the etags are equal
                // raise a 304 Not Modified Response
                if (incomingEtag.Equals(generatedEtag))
                {
                    executedContext.Result = new StatusCodeResult((int)HttpStatusCode.NotModified);
                }
            }

            // add ETag response header 
            response.Headers.Add(HeaderNames.ETag, new[] { generatedEtag });
        }

        private string GenerateEtagFromResponseBodyWithHash(Object serverProduct)
        {
            logger.LogInformation($"--- before Json: {serverProduct.GetType().Name}");
            var serverProductJson = JsonConvert.SerializeObject(serverProduct);
            logger.LogInformation($"--- After Json: {serverProductJson}");
            var objToken = JToken.Parse(serverProductJson);
            var hashCodeString = $"\"{equalityComparer.GetHashCode(objToken)}\"";
            logger.LogInformation($"--- hashcode string: {hashCodeString}");
            return hashCodeString;
        }

        private static bool IsSuccessStatusCode(int statusCode)
        {
            return statusCode >= 200 && statusCode <= 299;
        }
    }
}
