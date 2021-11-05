# SQL DB Application

Application to learn connection to SQL DB and perform CRUD operations

## Dependencies:
- Swashbuckle.AspNetCore
- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.Design
- Microsoft.EntityFrameworkCore.Tools
- Microsoft.EntityFrameworkCore.SqlServer


## Reference links:
- Scaffolding DB scema to Models
  - Install on dotnet CLI `dotnet tool install --global dotnet-ef`
  - Reference https://docs.microsoft.com/en-us/ef/core/managing-schemas/scaffolding?tabs=dotnet-core-cli

```
dotnet ef dbcontext scaffold "Data Source=KARTIKATL0717;Initial Catalog=SqlDbApplication;User ID=****;Password=******;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False" Microsoft.EntityFrameworkCore.SqlServer --table stores,products --context SqlRepositoryImpl --context-dir Repositories/Sql --output-dir Models/Sql
```