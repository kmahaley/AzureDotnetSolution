using DistributedCacheApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using Microsoft.VisualBasic;
using System.Net;

namespace DistributedCacheApplication.Filters
{
    public class HttpETagFilter : ActionFilterAttribute, IAsyncActionFilter
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext executingContext, ActionExecutionDelegate next)
        {
            var con = executingContext.Controller;
            var routeData = executingContext.RouteData;
            routeData.Values.TryGetValue("action", out object actionValue);
            routeData.Values.TryGetValue("controller", out object controllerValue);
            var request = executingContext.HttpContext.Request;
            var path = request.Path.ToString();


            var executedContext = await next();
            var response = executedContext.HttpContext.Response;

            // Computing ETags for Response Caching on GET requests
            //if (request.Method == HttpMethod.Get.Method && response.StatusCode == (int)HttpStatusCode.OK)
            if (response.StatusCode == (int)HttpStatusCode.OK)
            {
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
