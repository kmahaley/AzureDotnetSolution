# MongoDbApplication

ASP.NET Core CRUD API backed by MongoDB.

## Features

- Item CRUD endpoints under `/Item`.
- MongoDB repository registered through dependency-injection extensions.
- AutoMapper mappings between domain models and DTOs.
- Liveness endpoint at `/health`.
- Tagged readiness details at `/health/ready`.
- Swagger UI at `/swagger`.

## Configuration

Configure `MongoDbConfiguration` in `appsettings.json`, including the MongoDB connection string and database settings. Keep production credentials outside source control.

## Run

```powershell
dotnet run --project .\MongoDbApplication\MongoDbApplication.csproj
```
