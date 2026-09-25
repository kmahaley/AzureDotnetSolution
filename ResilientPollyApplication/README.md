# ResilientPollyApplication

ASP.NET Core API demonstrating resilient outbound HTTP calls with Polly.

## Patterns

- Named `HttpClient` registrations.
- Typed `HttpClient` services.
- Wrapped Polly policies.
- Retryable HTTP status handling.
- Timing and exception message handlers.

The `Book` and `Library` controllers expose endpoints that exercise the different client registrations. The configured downstream base addresses must point to a reachable service, such as `SimulateDownStreamApplication`.

## Run

```powershell
dotnet run --project .\ResilientPollyApplication\ResilientPollyApplication.csproj
```
