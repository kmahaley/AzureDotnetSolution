# AzureKeyVaultApplication

ASP.NET Core API demonstrating Azure Key Vault secret access and Application Insights integration.

## Features

- Key Vault access through `Azure.Identity` and `Azure.Security.KeyVault.Secrets`.
- Examples using developer credentials, managed identity, client secrets, and certificates.
- Configuration binding for `KeyVaultConfiguration` and `AADApplicationConfiguration`.
- API endpoints under `/API`, including `/API/keyvault`.

## Configuration

Populate the configuration sections in `appsettings.json` with non-secret identifiers such as vault URI, tenant ID, and application ID. Keep client secrets and certificate material outside source control by using environment variables, user secrets, or a managed identity.

The calling identity must have permission to read secrets from the target vault.

## Run

```powershell
dotnet run --project .\AzureKeyVaultApplication\AzureKeyVaultApplication.csproj
```
