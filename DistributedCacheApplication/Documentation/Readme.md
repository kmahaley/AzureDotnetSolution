# DistributedCacheApplication


## Middleware

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

## Filters

- Filter can be added after middleware pipeline calls endpoints. We can write code before/after the action
- Filter attributes are defined using 

```
public class HttpETagFilter : ActionFilterAttribute, IAsyncActionFilter
{
...
}
```

### GlobalFilter
- Executes on all controller actions of the applications

```
services.AddControllers(options => 
            {
                options.Filters.Add(new GlobalFilter("GlobalFilter"));
            });
```

### ControllerFilter

- Can be defined on just individual controller

```
[Route("api/[controller]")]
[ApiController]
[ControllerFilter("ProductController")]
public class ProductController : ControllerBase
``` 

### ActionFilter
- Runs before and after action of the controller

```
[HttpGet("{id}")]
[HttpETagFilter]
public async Task<ActionResult<Product>> GetAsync(int id)
```

## MemoryCache

- Memory cache is from namespace `Microsoft.Extensions.Caching.Memory`
 
### Add DI for MemoryCache
- `services.AddMemoryCache();`
- inject using `IMemoryCache memoryCache`

## Redis

### Docker

- install docker
- run `docker ps` and check docker is running
- create redis container using docker-compose or commandline

#### Docker compose
- on powershell, `cd DistributedCacheApplication\Docker` 
- `docker-compose up -d`
- you should see `redis` container running
- stop containers => `docker-compose stop`
- remove containers => `docker-compose down`

#### Docker command

- `docker run -p 6379:6379 --name redis -d redis`


#### Test Redis container
- Check container status
  - Docker desktop
    - Open docker desktop from windows app
    - check containers running
  - Visual studio code
    - install docker extension
    - check containers in left hand side menu. running/stopped status with conatiner name.
  - powershell
    - `docker ps`

- Test Redis using Redis CLI
  - `docker exec -it {conatinerName} bash` => `docker exec -it redis /bin/bash`
  - `redis-cli` => should connect to `127.0.0.1:6379`
  - `ping` => response should be `pong`


### NuGet package

- You can use Redis using either of the 2 ways
  - IDistributedCache 
    - with restricted datatypes and
    - `StackExchange.Redis` is included
    - `<PackageReference Include="Microsoft.Extensions.Caching.StackExchangeRedis" Version="6.0.12" />`

  - IConnectionMultiplexer with all datatypes
    - `<PackageReference Include="StackExchange.Redis" Version="2.6.86" />`


### Reference
- [Redis docker](https://docs.redis.com/latest/rs/installing-upgrading/get-started-docker/)
