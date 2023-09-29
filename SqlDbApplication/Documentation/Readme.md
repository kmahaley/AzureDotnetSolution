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
- Code 1st approach
  - `add-migration {NameOfTheMigration}`
  - `update-database`

## Saving ConnectionString or Credentials

### Using AppSettings.json
- Add connection string configuration as below in appsetting.json
```
  "ConnectionStrings": {
    "DefaultConnection":
```
- Based on Environment you can add connection string in different settings.json file

### using Environment Variable
- Windows -> Environment variable -> system Variable
- Create env variable
  - name: `ConnectionStrings:DefaultConnection`
  - value: connection string value
- ok and restart Visual studio

## Add database
1) Add models in the project. eg. Product, Store etc.
2) Add connection string configuration as below in appsetting.json
```
  "ConnectionStrings": {
    "DefaultConnection":
```


- Add Repository instance 
  - ConfigureServices -> `services.AddDbContext<SqlDatabaseContext>();`

## Database query logging

- if we want to test the SQL query and Log the query, we can add below in appsettings.json

```
"Microsoft.EntityFrameworkCore.Database.Command": "Information"
```

## Dispose Context Issue

### Demonstrate "DisposeContextIssue"
- ProductService wants to add and update the product on method `DisposeContextIssueAsync`.
- Add on main thread and update on new thread
- Main threads add data but new thread to update product show's error as `Disposed Dbcontext`
- This happens because DbContext or IProducRepository is scope instance hence new thread does not have access to it


### Solve "DisposeContextIssue"

- ProductService wants to add and update the product on method `SolveDisposeContextIssueAsync`.
- Add on main thread and update on new thread
- Main threads add data  and task to update product is given to `FireAndForgetService` which is singleton. FireAndForgetService creates scope of `IProductRepository` and update product

## BackgroundDatabaseService

- Implements `BackgroundService`
- Instance injected as `services.AddHostedService<BackgroundDatabaseService>();`
- This service starts executing task given to it as soon as application starts.
- `BackgroundDatabaseService` ExecuteAsync method get a product and starts updating it.

## Deferred Execution and Search

- When user provide search string and we have multiple column to search from use IQueryable<T>
- This build expression tree by appending multiple searching condition
- IQueryable<T> is not executed until terminal clause is called

### Using IQueryable<T>

- declared in repository layer as
`var queryCollection = databaseContext.Cities as IQueryable<City>;`
- Based on search column used as

```
if (!string.IsNullOrWhiteSpace(searchQuery))
{
    searchQuery = searchQuery.Trim();
    queryCollection = queryCollection
    .Where(city => city.Name.Contains(searchQuery) || city.Description.Contains(searchQuery));
}
```

- Keep appending using where() clause

### Terminal expression

- foreach loop
- ToListAsync(), ToArray() etc
- Singleton expression like
  - count()
  - avg()

### Pagination

- PaginationMetadata should contain following data along with the List of the Items/City 

```
class PaginationMetadata
{
public int TotalItemCount { get; set; }
public int TotalPageCount { get; set; }
public int PageSize { get; set; }
public int CurrentPage { get; set; }
}

class CityPageDto
{
public IEnumerable<CityDto> Cities { get; set; }
public PaginationMetadata PaginationMetadata { get; set; }
}
```

## Authentication of API

- In order to authenticate the API, we create JWT token

### JWT Token
- it contains 3 parts. Algorithm and token type, Payload for which token is generated, hash of the payload(signature)
- https://jwt.io/
```
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxIiwiZ2l2ZW5fbmFtZSI6IkpvaG4iLCJmYW1pbHlfbmFtZSI6IldpY2siLCJjaXR5IjoiU2VhdHRlIiwibmJmIjoxNjcyMzI2MTE1LCJleHAiOjE2NzIzMjk3MTUsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCIsImF1ZCI6InVzZXIifQ.EdatiUEfBTSZphCnJdBoEsxiz9BceRt4dkVvhInptkg
```

#### How to create

- For demo I am using keyvault as appsettings.json
- provide security key in appsettings.json
- Issuer data in appsettings.json
- check username/password against database
- Check `AuthenticationController` example
- Not for production application.
- Use OAuth2 and OpenId to secure production application

#### How to use
- Configure/Add authorization service
- use authorization middleware
- Annotate controller with `[Authorize]` attribute

## API Versioning

- Use package
`<PackageReference Include="Microsoft.AspNetCore.Mvc.Versioning" Version="5.0.0" />`
- Configure/Add versioning service

```
services.AddApiVersioning(setupAction =>
{
    setupAction.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    setupAction.ReportApiVersions = true;
    setupAction.AssumeDefaultVersionWhenUnspecified = true;
});
```
- Resonpse will contain `api-supported-versions:1.0`

- Annotate controller with `[ApiVersion("1.0")]`


## API documentation

- Add swagger, which is out of the box in dotnet core application

### Generate documentation from XML file

- Properties of the project -> Build 
- XML documentation file path == NameOfTheProject.xml, example `SqlDbApplication.xml`
- Add XML documentation on the DTOs, object and controller APIs
- Configure swagger as below

```
services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1",
        new OpenApiInfo()
        {
            Title = SwaggerApiName,
            Version = "v1",
            Description = "Use this application to learn SQL DB connection"
        });

    //Generate comments using XML file
    var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
    var xmlCommentsFile = $"{assemblyName}.xml";
    var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
    options.IncludeXmlComments(xmlCommentsFullPath);
});

```

## Entity and Child navigation

### Example of 
- `1 to many`, `1 to 1` and `many to many` relationship
- Collection navigation
- Reference navigation
- Foreign key

```
public class Blog
{
    public int Id { get; set; } // Primary key
    public string Name { get; set; }

    public IList<Post> Posts { get; } = new List<Post>(); // Collection navigation
    public BlogAssets Assets { get; set; } // Reference navigation
}

public class BlogAssets
{
    public int Id { get; set; } // Primary key
    public byte[] Banner { get; set; }

    public int? BlogId { get; set; } // Foreign key
    public Blog Blog { get; set; } // Reference navigation
}

public class Post
{
    public int Id { get; set; } // Primary key
    public string Title { get; set; }
    public string Content { get; set; }

    public int? BlogId { get; set; } // Foreign key
    public Blog Blog { get; set; } // Reference navigation

    public IList<Tag> Tags { get; } = new List<Tag>(); // Skip collection navigation
}

public class Tag
{
    public int Id { get; set; } // Primary key
    public string Text { get; set; }

    public IList<Post> Posts { get; } = new List<Post>(); // Skip collection navigation
}
```

- Move post from 1 blog to another, 3 ways to acjieve this
  - collection navigation
  - reference navigation
  - FK adjustment
  
```
//collection navigation
using var context = new BlogsContext();

var dotNetBlog = context.Blogs.Include(e => e.Posts).Single(e => e.Name == ".NET Blog");
var vsBlog = context.Blogs.Include(e => e.Posts).Single(e => e.Name == "Visual Studio Blog");

Console.WriteLine(context.ChangeTracker.DebugView.LongView);

var post = vsBlog.Posts.Single(e => e.Title.StartsWith("Disassembly improvements"));
vsBlog.Posts.Remove(post);
dotNetBlog.Posts.Add(post);

context.ChangeTracker.DetectChanges();
Console.WriteLine(context.ChangeTracker.DebugView.LongView);

context.SaveChanges();
```

```
//reference navigation
var post = vsBlog.Posts.Single(e => e.Title.StartsWith("Disassembly improvements"));
post.Blog = dotNetBlog;
```

```
//FK value
var post = vsBlog.Posts.Single(e => e.Title.StartsWith("Disassembly improvements"));
post.BlogId = dotNetBlog.Id;
```

## References

- [Change tracking in Database context](https://learn.microsoft.com/en-us/ef/core/change-tracking/)
- [Relationships](https://learn.microsoft.com/en-us/ef/core/modeling/relationships?tabs=fluent-api%2Cfluent-api-simple-key%2Csimple-key)
- [Relationships changes: Changing Foreign Keys and Navigations](https://learn.microsoft.com/en-us/ef/core/change-tracking/relationship-changes)
- [Cascade delete](https://learn.microsoft.com/en-us/ef/core/saving/cascade-delete)