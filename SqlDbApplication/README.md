# SqlDbApplication

ASP.NET Core API demonstrating SQL Server access with Entity Framework Core, repository/service layers, AutoMapper, migrations, authentication examples, and API versioning.

## API areas

- Cities and points of interest.
- Products and store/context-lifetime demonstrations.
- Private endpoint metadata.
- Authentication/token examples.
- Swagger UI at `/swagger`.

Most controller routes use the `/api/<controller>` convention.

## Configuration

Set the SQL Server connection string in `ConnectionStrings` through local configuration, user secrets, or environment variables. Do not commit database credentials.

Apply migrations with:

```powershell
dotnet ef database update --project .\SqlDbApplication\SqlDbApplication.csproj
```

## Run

```powershell
dotnet run --project .\SqlDbApplication\SqlDbApplication.csproj
```
