# SQL DB Application

Application to learn connection to SQL DB and perform CRUD operations

## Dependencies:
- Swashbuckle.AspNetCore
- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.Design
- Microsoft.EntityFrameworkCore.Tools
- Microsoft.EntityFrameworkCore.SqlServer

*EntityFrameworkCore packages should be of same version*

## Connect to database
- Install SQL server and Microsoft SQL server management studio
- Connect "localhost" database using username and password mentioned in appsetting.json


## Reference links:
- Scaffolding DB scema to Models
  - Install on dotnet CLI `dotnet tool install --global dotnet-ef`
  - Add connection string configuration as below in appsetting.json
    ```
    "ConnectionStrings": {
        "DefaultConnection":
    ```
  - Add instance of SqlRepositoryImpl in DI framework and use configurations
    ```
    services.AddDbContext<SqlRepositoryImpl>(option => 
                option.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
    ```
  - 2 ways to create Database. Database first or Code first. Reference .
    - Database 1st: 
      1) Reference: https://docs.microsoft.com/en-us/ef/core/managing-schemas/scaffolding?tabs=dotnet-core-cli
      2) Create SQL tables and then run command in dotnet CLI
        ```
        dotnet ef dbcontext scaffold "Data Source=KARTIKATL0717;Initial Catalog=SqlDbApplication;User ID=***;Password=***;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False" Microsoft.EntityFrameworkCore.SqlServer --table stores,products --context SqlRepositoryImpl --context-dir Repositories/Sql --output-dir Models/Sql
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


## Add database
1) Add models in the project. eg. Product, Store etc.
2) Add connection string configuration as below in appsetting.json
```
  "ConnectionStrings": {
    "DefaultConnection":
```




- Add Repository instance 
  - ConfigureServices -> `services.AddDbContext<SqlRepositoryImpl>();`