# CoreConsoleApplication

A collection of console-based .NET experiments and reusable demonstrations.

## Areas covered

- Async and task behavior.
- .NET framework and project dependency inspection.
- SQL Server and Entity Framework concurrency examples.
- BenchmarkDotNet experiments.
- Collection, equality, sorting, ETag, subnet, and subscription utilities.
- File processing and PowerShell SDK examples.

`Program.Main` is a scratch-pad entry point. Enable or invoke the desired utility explicitly; several archived methods require local files, SQL Server, or other environment-specific configuration.

## Run

```powershell
dotnet run --project .\CoreConsoleApplication\CoreConsoleApplication.csproj
```

Database examples require a valid SQL Server connection string. Do not embed credentials in source code.
