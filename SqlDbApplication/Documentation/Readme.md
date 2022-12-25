# SQL DB Application

Application to learn connection to SQL DB and perform CRUD operations

## Dependencies:
- Swashbuckle.AspNetCore
- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.Design
- Microsoft.EntityFrameworkCore.Tools
- Microsoft.EntityFrameworkCore.SqlServer

*EntityFrameworkCore packages should be of same version*

## SQL Server database

### Install SQL Server
- Install docker desktop
  - if docker desktop failed to start. Error "failed to initialize or shutting down"
  - Go to file path `C:\Users\kamahale\AppData\Roaming` and delete Docker folder.
  - folder to delete `C:\Users\kamahale\AppData\Roaming\Docker` 
- Get SQL image and create container from it using below command

```
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Password123" -p 1433:1433 --name sql --hostname sqlhostname -d mcr.microsoft.com/mssql/server:2022-latest
```
- then run `docker ps` command, you will see SQL server running
- Reference
  - https://learn.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker?view=sql-server-ver16&preserve-view=true&pivots=cs1-bash

### Connect to SQL Server


- Using Microsoft SQL server management studio
  - Install SSMS and open SSMS, provide below details
  - ServerType: Database engine
  - ServerName: localhost,1433
  - SQL server authentication
  - username/password: sa:Password123 => this is provided in docker command
- Using VisualStudio 2022
  - view-> SQL Server Object explorer
  - connect and provide details as above
- Reference
  - https://learn.microsoft.com/en-us/sql/linux/sql-server-linux-manage-ssms?view=sql-server-ver16

### Create Database
- Create database same as application name `SqlDbApplication` using SSMS

## Reference links
- Scaffolding DB scema to Models
### Using dotnet CLI 
- Install on dotnet CLI `dotnet tool install --global dotnet-ef`
- Add connection string configuration as below in appsetting.json
  ```
  "ConnectionStrings": {
      "DefaultConnection":
  ```
- Add instance of SqlDatabaseContext in DI framework and use configurations
  ```
  services.AddDbContext<SqlDatabaseContext>(option => 
              option.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
  ```
- 2 ways to create Database. Database first or Code first. Reference .
  - Database 1st: 
    1) Reference: https://docs.microsoft.com/en-us/ef/core/managing-schemas/scaffolding?tabs=dotnet-core-cli
    2) Create SQL tables and then run command in dotnet CLI
      ```
      dotnet ef dbcontext scaffold "Data Source=localhost;Initial Catalog=SqlDbApplication;User ID=***;Password=***;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False" Microsoft.EntityFrameworkCore.SqlServer --table stores,products --context SqlDatabaseContext --context-dir Repositories/Sql --output-dir Models/Sql
      ```
  - Code 1st: 
    1) Reference: https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli
    2) Add model in C# project. eg Store.cs, Product.cs
    3) use dotnet CLI to create scripts
      ```
      dotnet ef migrations add {InitialScriptName}
      dotnet ef database update
      ```
    4) You can update model and run script again in same way
      ```
      dotnet ef migrations add {UpdateScriptName}
      dotnet ef database update
      ```

### Using Package manager console
- You can you package manager console too instead of dotnet cli.
- VS2022 -> Tools > NuGet Package Manager > Package Manager Console 
- https://learn.microsoft.com/en-us/ef/core/cli/powershell
- `add-migration {NameoFtheMigration}` -> `update-database`

## Add database
1) Add models in the project. eg. Product, Store etc.
2) Add connection string configuration as below in appsetting.json
```
  "ConnectionStrings": {
    "DefaultConnection":
```


- Add Repository instance 
  - ConfigureServices -> `services.AddDbContext<SqlDatabaseContext>();`


## Demonstrate "DisposeContextIssue"
- ProductService wants to add and update the product on method `DisposeContextIssueAsync`.
- Add on main thread and update on new thread
- Main threads add data but new thread to update product show's error as `Disposed Dbcontext`
- This happens because DbContext or IProducRepository is scope instance hence new thread does not have access to it


## Solve "DisposeContextIssue"

- ProductService wants to add and update the product on method `SolveDisposeContextIssueAsync`.
- Add on main thread and update on new thread
- Main threads add data  and task to update product is given to `FireAndForgetService` which is singleton. FireAndForgetService creates scope of `IProductRepository` and update product

## BackgroundDatabaseService

- Implements `BackgroundService`
- Instance injected as `services.AddHostedService<BackgroundDatabaseService>();`
- This service starts executing task given to it as soon as application starts.
- `BackgroundDatabaseService` ExecuteAsync method get a product and starts updating it.
