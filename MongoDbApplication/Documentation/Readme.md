# MongoDbApplication

[Dotnet core Microsoft docs](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/?view=aspnetcore-5.0&tabs=windows)

## Docker
- [Install docker](https://docs.docker.com/get-docker/)
- Test docker installed by commandline -> `docker info`

## MongoDb
- command line: 
  - `docker run -d --rm --name mongodb -p 27017:27017 -v mongodata:/data/db mongo`
  - Test docker instance: `docker ps`
  - Test via browser -> `localhost:27017`
  - Install VSCode extension Mongodb -> provide connection string and connect

### Reference: 
- [MongoDB APIs](https://chsakell.gitbook.io/mongodb-csharp-docs/getting-started/quick-start/databases)
- [MongoDB tutorial](https://www.youtube.com/watch?v=ZXdFisA_hOY&t=6943s&ab_channel=freeCodeCamp.org)

## Secrets

Passwords can not be saved in appsettings.json file. use keyvaults/Environment variables/Command line parameters. here we are using **dotnet secret manager**

- goto commanline of the project and run
```
dotnet user-secrets init
dotnet user-secrets set {appsettings.json} {VALUE}
eg.
dotnet user-secrets set MongoDbConfiguration:Password Value123
```
check `.csproj` file and you will see `<UserSecretsId>15e47230-9b6d-4cdd-b94c-372652746ef7</UserSecretsId>` added.</br>
also mongodb connection string will be `mongodb://{Username}:{Password}@{Host}:{Port}`

## Automapper
Add mapping from 1 object to another object 
- packages
```
<PackageReference Include="AutoMapper" Version="10.1.1" />
<PackageReference Include="AutoMapper.Extensions.Microsoft.DependencyInjection" Version="8.1.1" />
```
- create profile `MapperProfile`
- add singleton `services.AddAutoMapper(typeof(MapperProfile));`

### References
- [Automapper](https://docs.automapper.org/en/stable/Getting-started.html)
- [Automapper eg.](https://dotnettutorials.net/lesson/automapper-with-nested-types/)