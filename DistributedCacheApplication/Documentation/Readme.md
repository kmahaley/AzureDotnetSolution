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

### Filter attributes

- Attributes can be added after middleware pipeline calls endpoints. We can write code before/after the action
- Filter attributes are defined using 
```
public class HttpETagFilter : ActionFilterAttribute, IAsyncActionFilter
{
...
}
```

#### GlobalFilter
- Executes on all controller actions of the applications

```
services.AddControllers(options => 
            {
                options.Filters.Add(new GlobalFilter("GlobalFilter"));
            });
```

#### ControllerFilter

- Can be defined on just individual controller

```
[Route("api/[controller]")]
[ApiController]
[ControllerFilter("ProductController")]
public class ProductController : ControllerBase
``` 

#### ActionFilter
- Runs before and after action of the controller

```
[HttpGet("{id}")]
[HttpETagFilter]
public async Task<ActionResult<Product>> GetAsync(int id)
```

### Redis

