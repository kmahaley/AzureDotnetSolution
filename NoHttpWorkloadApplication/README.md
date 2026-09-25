# NoHttpWorkloadApplication

A .NET Generic Host sample for workloads that do not expose an HTTP endpoint.

The host registers:

- `BackgroundHostedBookService`
- `BackgroundBookService`

Both services start with the process and demonstrate hosted/background execution and cancellation behavior. The application currently forces the `Development` environment in `Program.cs`.

## Run

```powershell
dotnet run --project .\NoHttpWorkloadApplication\NoHttpWorkloadApplication.csproj
```

Stop the host with `Ctrl+C` to exercise graceful cancellation.
