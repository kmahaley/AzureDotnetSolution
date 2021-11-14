# CoreWebApplication

[Dotnet core Microsoft docs](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/?view=aspnetcore-5.0&tabs=windows)

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

**Injecting multiple implementation of the service**

```
ConfirgureServices() ....
services.AddSingleton<IMyDependency, MyDependency>();
services.AddSingleton<IMyDependency, DifferentDependency>();

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

**Priority:**
 -  commandline args
 -  environment variables
 -  appsettings.{EnvironmentName}.json -> appsettings.Development.json
 -  appsettings.json

appsettings.json is kept as `reloadOnChange: true` in HostBuilder

**Injecting configurations**

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

**Environment variables**: IWebHostEnvironment 

## Logging
`ILogger<TodoController> logger`

## Docker
- [Install docker](https://docs.docker.com/get-docker/)
- Test docker installed by commandline -> `docker info`

### MongoDb
- command line: 
  - `docker run -d --rm --name mongodb -p 27017:27017 -v mongodata:/data/db mongo`
  - Test docker instance: `docker ps`
  - Test via browser -> `localhost:27017`
  - Install VSCode extension Mongodb -> provide connection string and connect

