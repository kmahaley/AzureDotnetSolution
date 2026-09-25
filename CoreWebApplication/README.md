# CoreWebApplication

ASP.NET Core API demonstrating common web application patterns with an in-memory repository.

## Features

- CRUD-style item endpoints under `/Item`.
- Response and fire-and-forget controller examples.
- Custom request-inspection middleware.
- Optional hosted background services.
- Serilog console and rolling-file configuration.
- Health endpoint at `/health`.
- Swagger UI at `/swagger`.

## Run

```powershell
dotnet run --project .\CoreWebApplication\CoreWebApplication.csproj
```

Data is stored in memory and is reset when the process restarts. Background services in `Startup.cs` are disabled by default and can be enabled individually for experimentation.
