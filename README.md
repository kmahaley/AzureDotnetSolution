# AzureDotnetSolution

A .NET 10 sample solution containing web APIs, console utilities, Azure resource-management examples, persistence samples, caching demonstrations, resilience patterns, background workers, and tests.

## Prerequisites

- .NET SDK 10.0.401 or a compatible .NET 10 SDK
- Optional local services, depending on the project:
  - SQL Server for `SqlDbApplication` and database-concurrency samples
  - MongoDB for `MongoDbApplication`
  - Redis for `DistributedCacheApplication`
  - Azure credentials and resources for the Azure projects

The SDK version is pinned by `global.json`.

## Projects

| Project | Purpose |
| --- | --- |
| `AzureConsoleApplication` | Azure ARM SDK examples for VMs, managed disks, networking, Compute Gallery images, and Private DNS dependencies. |
| `AzureKeyVaultApplication` | ASP.NET Core API demonstrating Azure Key Vault access with several credential approaches. |
| `CoreConsoleApplication` | Console-based .NET, dependency, benchmark, SQL concurrency, and utility experiments. |
| `CoreWebApplication` | Core ASP.NET API patterns, middleware, in-memory repositories, health checks, and background services. |
| `DistributedCacheApplication` | In-memory, distributed, and Redis cache APIs with filters and health checks. |
| `MongoDbApplication` | MongoDB CRUD API with AutoMapper and readiness/health endpoints. |
| `NoHttpWorkloadApplication` | Generic Host application running background services without HTTP. |
| `ResilientPollyApplication` | Named, typed, and policy-wrapped HTTP client resilience examples. |
| `SimulateDownStreamApplication` | Configurable downstream API used to exercise HTTP and resilience behavior. |
| `SqlDbApplication` | SQL Server API using Entity Framework Core, repositories, services, migrations, and API versioning. |
| `UtilityLibrary` | Shared Polly policy and context helpers. |
| `CoreWebApplication.UnitTests` | MSTest and Moq unit tests for `CoreWebApplication`. |
| `CoreWebApplication.IntegrationTests` | ASP.NET Core integration-test project for `CoreWebApplication`. |

Each project has its own README with configuration and usage details.

## Build and test

```powershell
dotnet restore .\AzureDotnetSolution.sln
dotnet build .\AzureDotnetSolution.sln
dotnet test .\AzureDotnetSolution.sln
```

Run an individual project with:

```powershell
dotnet run --project .\<project-directory>\<project-file>.csproj
```

Swagger-enabled APIs expose their UI at `/swagger` unless otherwise noted.

## Configuration

Configuration is supplied through each application's `appsettings.json`, environment-specific settings, user secrets, or environment variables. Do not commit credentials. Use local secret storage or managed identity for Azure resources and secure connection-string providers for databases and caches.

Docker Compose examples for SQL Server, MongoDB, and Redis are available under `Docker`.
