# CoreWebApplication

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

Injecting multiple implementation of the service

```
ConfirgureServices() ....
services.AddSingleton<IMyDependency, MyDependency>();
services.AddSingleton<IMyDependency, DifferentDependency>();

...

public class MyService
{
    public MyService(IMyDependency myDependency, IEnumerable<IMyDependency> myDependencies) {

        myDependency will be of type DifferentDependency    

    }

```
