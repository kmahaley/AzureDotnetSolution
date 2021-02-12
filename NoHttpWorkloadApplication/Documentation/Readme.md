# NoHttpWorkloadApplication

## Build

  - `git clone https://github.com/kmahaley/AzureDotnetSolution.git`
  - cd into AzureDotnetSolution -> NoHttpWorkloadApplication
  - run following commands
    - `dotnet build`
    - `dotnet run`
    - Verify terminal logs

## Fundamentals

  - Http workload:
```
public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });  
```

  - Non http workload:
```
public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
               services.AddHostedService<Worker>();
            }); 
```

## References
  - https://docs.microsoft.com/en-us/aspnet/core/fundamentals/?view=aspnetcore-5.0&tabs=windows