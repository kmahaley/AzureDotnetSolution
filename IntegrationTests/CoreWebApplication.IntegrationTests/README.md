# CoreWebApplication.IntegrationTests

ASP.NET Core integration-test project for `CoreWebApplication`, using `Microsoft.AspNetCore.Mvc.Testing` and MSTest.

The current test fixture establishes the structure for hosting the application through `WebApplicationFactory<Startup>`. Expand the existing placeholder test with HTTP requests and response assertions as integration scenarios are added.

## Run

```powershell
dotnet test .\IntegrationTests\CoreWebApplication.IntegrationTests\CoreWebApplication.IntegrationTests.csproj
```
