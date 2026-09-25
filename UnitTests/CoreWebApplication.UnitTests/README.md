# CoreWebApplication.UnitTests

MSTest unit tests for `CoreWebApplication`.

The tests use Moq to isolate `ItemController` from its repository and logger dependencies. Current coverage includes missing-item responses, successful item retrieval, and list retrieval.

## Run

```powershell
dotnet test .\UnitTests\CoreWebApplication.UnitTests\CoreWebApplication.UnitTests.csproj
```
