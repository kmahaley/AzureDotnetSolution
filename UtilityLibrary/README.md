# UtilityLibrary

Shared resilience helpers used by the sample applications.

## Contents

- Polly constants and retryable-result definitions.
- Context extensions.
- Named and typed `HttpClient` policy helpers.
- Non-HTTP policy examples.

The library targets .NET 10 and references `Microsoft.Extensions.Http.Polly`.

## Build

```powershell
dotnet build .\UtilityLibrary\UtilityLibrary.csproj
```
