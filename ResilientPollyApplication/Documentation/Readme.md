# ResilientPollyApplication

## How to setup project

- Add project dependency <b>UtilityLibrary</b> to your application.
- Create Named or Typed http client. Please refer https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests 
- Add Policies from `ResilientPolicies.cs`

## Add Policies
 Once you have selected Name or Typed http client. In your `Startup.cs` class add below mentioned code.

Typed http client implementation:
```
services.AddHttpClient<IBasketService, BasketService>()
        ...
        .AddPolicyHandler(ResilientPolicies.CreateTimeoutPolicy(TimeSpan.FromSeconds(20)))
        .AddPolicyHandler(ResilientPolicies.CreateWaitAndRetryPolicy());
```

Named http client implementation:
```
services.AddHttpClient("NamedHttpClient")
        ...
        .AddPolicyHandler(ResilientPolicies.CreateTimeoutPolicy(TimeSpan.FromSeconds(20)))
        .AddPolicyHandler(ResilientPolicies.CreateWaitAndRetryPolicy());
```

***Policy ordering matters***

### CircuitBreaker policy

If you have applied circuit breaker policy to Http client then make sure its the single instance shared and not created per request.
Creating single instance per Http client will block entire host when threshold of failures are reached.

If you want to apply circuit breaker to single Uri without affecting others. Then create circuit breaker instance per Uri.

```
policy1 = NamedHttpClientBasedPolicy.CreateCircuitBreakerPolicy(durationOfTheBreak: TimeSpan.FromSeconds(30));
policy2 = NamedHttpClientBasedPolicy.CreateCircuitBreakerPolicy(durationOfTheBreak: TimeSpan.FromSeconds(30));


response = await policy2.ExecuteAsync( () => httpClient.SendAsync(request));
``` 

Explaination: You have 2 APIs, 
- Post: http://service.com/book 
- Get: http://service.com/book/{Id}

Post has complex SQL query running and database is struggle to execute complex query but Get is simple select query, then you can apply
policy1 to Post and policy2 to Get.

This will maintain separate state of calls and if one fails it won't necessarily block other Uri

## References
- https://github.com/App-vNext/Polly
- https://github.com/App-vNext/Polly/wiki/Keys-and-Context-Data
- https://github.com/App-vNext/Polly/wiki/Polly-and-HttpClientFactory
- https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests