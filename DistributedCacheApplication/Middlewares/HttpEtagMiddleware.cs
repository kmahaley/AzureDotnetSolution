using Microsoft.Net.Http.Headers;
using System.Net;

namespace DistributedCacheApplication.Middlewares
{
    public class HttpEtagMiddleware : IMiddleware
    {
        private readonly ILogger<HttpEtagMiddleware> logger;

        public HttpEtagMiddleware(ILogger<HttpEtagMiddleware> logger)
        {
            this.logger = logger;
        }


        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {
            // user request and headers
            logger.LogInformation("inside HttpEtagMiddleware");
            var userRequest = httpContext.Request;
            await next(httpContext);
            var serverResponse = httpContext.Response;

            var resultBody = serverResponse.Body;
            var calculatedEtag = GenerateEtagFromResponseBodyWithHash(resultBody);

            if (userRequest.Method.Equals("GET") && userRequest.Path.Equals("/api/product"))
            {
                if (userRequest.Headers.ContainsKey(HeaderNames.IfNoneMatch))
                {
                    var userRequestedEtag = userRequest.Headers[HeaderNames.IfNoneMatch].FirstOrDefault();
                    if (string.Equals(userRequestedEtag, calculatedEtag, StringComparison.OrdinalIgnoreCase))
                    {
                        // data not modified hence http 304 code 
                        httpContext.Response.StatusCode = (int)HttpStatusCode.NotModified;
                    }
                }

                
            }

            httpContext.Response.OnStarting(() =>
            {
                serverResponse.Headers.Add(HeaderNames.ETag, calculatedEtag);
                return Task.CompletedTask;
            });

            // next middleware in pipeline
        }

        private string GenerateEtagFromResponseBodyWithHash(Stream result)
        {
            return "";
        }
    }
}
