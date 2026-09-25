# SimulateDownStreamApplication

ASP.NET Core API that behaves as a downstream dependency for HTTP-client, retry, and failure-handling experiments.

## Endpoints

- `/student` provides CRUD-style and mock response operations.
- `/subject` accepts multiple HTTP methods and catch-all resource paths.
- `/swagger` provides interactive API documentation.

The in-memory student repository is registered as a singleton, so state lasts for the lifetime of the process.

## Run

```powershell
dotnet run --project .\SimulateDownStreamApplication\SimulateDownStreamApplication.csproj
```

Point `ResilientPollyApplication` client configuration at this application's launch URL when testing resilience behavior.
