﻿# CoreWebApplication

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
 -  appsettings.[EnvironmentName].json -> appsettings.Development.json
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

## Secrets

Passwords can not be saved in appsettings.json file. use keyvaults/Environment variables/Command line parameters. here we are using **dotnet secret manager**

- goto commanline of the project and run
```
dotnet user-secrets init
dotnet user-secrets set {NAME} {VALUE}
eg.
dotnet user-secrets set MongoDbConfiguration:Password Value123
```
check `.csproj` file and you will see `<UserSecretsId>15e47230-9b6d-4cdd-b94c-372652746ef7</UserSecretsId>` added.</br>
also mongodb connection string will be `mongodb://{Username}:{Password}@{Host}:{Port}`


