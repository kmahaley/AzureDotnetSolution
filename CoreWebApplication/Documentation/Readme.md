# CoreWebApplication

## References
- [CSharp Language](https://learn.microsoft.com/en-us/dotnet/csharp/)
- [Dotnet APIs](https://learn.microsoft.com/en-us/dotnet/api/)
- [Dotnet core Microsoft docs](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/?view=aspnetcore-5.0&tabs=windows)
- [Github of aspnetcore source code](https://github.com/dotnet/aspnetcore)

## Startup class constructor

Injectable services are
```
IWebHostEnvironment
IHostEnvironment
IConfiguration
```

## Registering Services using ConfirgureServices

```
services.AddSingleton<IMyDependency, MyDependency>();
services.AddScoped<IMyDependency, MyDependency>();
services.AddTransient<IMyDependency, MyDependency>();
```

### Injecting multiple implementation of the service

```
ConfirgureServices() ....
services.AddSingleton<IMyDependency, MyDependency>();
services.AddSingleton<IMyDependency, DifferentDependency>();

...
...

public class MyService
{
    public MyService(IMyDependency myDependency, IEnumerable<IMyDependency> myDependencies) {

        myDependencies.First(dep => nameOf(DifferentDependency).Equals(dep.GetType().Name))   

    }


```
## Configurations

- [Generic configuration](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-5.0#evcp)
- [Option bindings](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-5.0#ios)

### Priority
 -  commandline args
 -  environment variables
 -  appsettings.[EnvironmentName].json -> appsettings.Development.json
 -  appsettings.json

appsettings.json is kept as `reloadOnChange: true` in HostBuilder

### Injecting configurations

```
Class KeyVaultConfiguration {...}

ConfigureServices(IServiceCollection services) {
    services.Configure<KeyVaultConfiguration>(Configuration.GetSection("KeyVaultConfiguration"));
}

Class KeyvaultService {

    private readonly KeyVaultConfiguration keyVaultConfiguration;

    public KeyVaultService(IOptions<KeyVaultConfiguration> keyVaultConfiguration)
        {
            this.keyVaultConfiguration = keyVaultConfiguration.Value;   
        }
}
```
you can use `IOptionsSnapshot<> or IOptionsMonitor<> ` to reload values on change

### Environment variables
- IWebHostEnvironment 

## Logging

### Default logger by
- `ILogger<TodoController> controllerLogger`
- `ILogger<TodoService> serviceLogger`
- You can add new logger provider that can write to file, database or storage account.
 
### Adding Serilog

#### Packages
- Sink is where to log the data. eg ApplicationInsights, ElasticSearch, file etc
- we use file and console

```
<!--Serilog dependency-->
<PackageReference Include="Serilog.AspNetCore" Version="6.1.0" />
<PackageReference Include="Serilog.Sinks.Console" Version="4.1.0" />
<PackageReference Include="Serilog.Sinks.File" Version="5.0.0" />
```

#### Configuring and using

- Configure logger
  - Startup.cs
  - `Logs/CoreWebApplication.txt` is location and name of the file generated daily
```
// Serilog configuration
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("Logs/CoreWebApplication.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

OR
// Add configuration in appsetting.json 
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();
```

- Instruct application builder to use Serilog, program.cs
```
Host
    .CreateDefaultBuilder(args)
    .UseSerilog()
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseStartup<Startup>();
    });
```

## Secrets

- Passwords can not be saved in appsettings.json file. use **keyvaults/Environment variables/Command line parameters**. 
- here we are using **dotnet secret manager**

  - goto command line of the project and run

```
dotnet user-secrets init
dotnet user-secrets set {NAME} {VALUE}

eg.
dotnet user-secrets set MongoDbConfiguration:Password Value123
```

- check `.csproj` file and you will see `<UserSecretsId>15e47230-9b6d-4cdd-b94c-372652746ef7</UserSecretsId>` added.</br>
- also mongodb connection string will be `mongodb://{Username}:{Password}@{Host}:{Port}`


## Creation of dependency from other dependencies

- if you have 2 service from same interface
```
interface IWeatherService {...}
class HttpWeatherService : IWeatherService {...}

class CacheWeatherService : IWeatherService
{
    CacheWeatherService(HttpWeatherService httpWeatherService, IMemoryCache memorycache)
}

....
services.AddSingleton<HttpWeatherService>();
services.AddSingleton<IWeatherService>(
    sp => { 
            var hws = sp.GetRequiredService<HttpWeatherService>();
            var mc = sp.GetRequiredService<ImemoryCache>(); 
            new CacheWeatherService(hws, mc);

});



```