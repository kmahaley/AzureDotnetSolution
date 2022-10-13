# DistributedCacheApplication

## Features

### Middleware

- applicationBuilder.Run(...) 
  - Used for terminating middlerware
- applicationBuilder.Use(...)
  - Used to add new middleware in the pipeline
- applicationBuilder.UseWhen(isPredicateMatched, HandleEtagMiddleware)
  - Used to match condition to add middleware in pipeline
  - once the conditional middleware execute continue with pipeline
- applicationBuilder.Map("/api/product", HandleEtagMiddleware)
  - Branch out middleware
  - pipeline is not resumed. new flow is followed
- applicationBuilder.MapWhen(isPredicateMatched, HandleEtagMiddleware)
  - Branch out middleware on predicate


```
private static bool isPredicateMatched(HttpContext httpContext)
{
   return httpContext.Request.Path.ToString().Contains("/api/product");
}
```
### Redis

