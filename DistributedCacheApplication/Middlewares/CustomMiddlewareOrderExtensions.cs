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
            app.UseMiddleware<HttpEtagMiddleware>();
            //app.Map("/api/product", HandleEtag);
        }

        public static void HandleEtag(IApplicationBuilder app)
        {
            app.UseMiddleware<HttpEtagMiddleware>();
        }
    }
}
