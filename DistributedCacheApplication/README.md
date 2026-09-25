# DistributedCacheApplication

ASP.NET Core API demonstrating in-memory caching, `IDistributedCache`, direct Redis access, action filters, throttling middleware, and health checks.

## Prerequisites

Start Redis locally on port `6379`, or update `ConnectionStrings:Redis` in `appsettings.json`.

Docker Compose examples are available in the repository's `Docker` directory.

## Endpoints

- `/InMemoryCache` for in-process caching examples.
- `/DistributedCache` for `IDistributedCache` examples.
- `/Redis` for direct StackExchange.Redis examples.
- `/api/DemonstrateFilter` for filter behavior.
- `/health` for Redis and SQL health-check output.
- `/swagger` for API documentation.

## Run

```powershell
dotnet run --project .\DistributedCacheApplication\DistributedCacheApplication.csproj
```
