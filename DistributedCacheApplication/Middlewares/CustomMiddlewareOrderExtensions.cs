namespace DistributedCacheApplication.Middlewares
{
    public static class CustomMiddlewareOrderExtensions
    {
        /// <summary>
        /// Configures custom middleware. Order is important while defining the middleware.
        /// ExceptionMiddleware should always be first.
        /// </summary>
        /// <param name="app">application.</param>
        public static void ConfigureApplicationCustomMiddleware(this IApplicationBuilder app)
        {
            //app.UseMiddleware<HttpEtagMiddleware>();
            app.UseWhen(isPredicateMatched, HandleEtag);
        }


        public static void HandleEtag(IApplicationBuilder app)
        {
            app.UseMiddleware<HttpEtagMiddleware>();
        }
        private static bool isPredicateMatched(HttpContext httpContext)
        {
            return httpContext.Request.Path.ToString().Contains("/api/product");
        }
    }
}
