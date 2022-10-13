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
            logger.LogInformation("--- before HttpEtagMiddleware");
            await next(httpContext);

            // user request and headers
            logger.LogInformation("--- after other middleware. inside HttpEtagMiddleware");
            var userRequest = httpContext.Request;
            var serverResponse = httpContext.Response;

            
            

            if (userRequest.Path.ToString().Contains("/api/product"))
            {
                logger.LogInformation("--- HttpEtagMiddleware. Product endpoint");
                var resultBody = serverResponse.Body;
                var calculatedEtag = GenerateEtagFromResponseBodyWithHash(resultBody);

                if (userRequest.Method.Equals("GET") && userRequest.Headers.ContainsKey(HeaderNames.IfNoneMatch))
                {
                    var userRequestedEtag = userRequest.Headers[HeaderNames.IfNoneMatch].FirstOrDefault();
                    if (string.Equals(userRequestedEtag, calculatedEtag, StringComparison.OrdinalIgnoreCase))
                    {
                        // data not modified hence http 304 code 
                        httpContext.Response.StatusCode = (int)HttpStatusCode.NotModified;
                    }
                }

                //if (!serverResponse.HasStarted)
                //{
                //    serverResponse.Headers[HeaderNames.ETag] = calculatedEtag;
                //}
                //else
                //{
                //    httpContext.Response.OnStarting(() =>
                //    {
                //        serverResponse.Headers[HeaderNames.ETag] = calculatedEtag;
                //        return Task.CompletedTask;
                //    });
                //}
                
            }

            

            // next middleware in pipeline
        }

        private string GenerateEtagFromResponseBodyWithHash(Stream result)
        {
            return "banana";
        }
    }
}
