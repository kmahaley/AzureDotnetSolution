# AzureConsoleApplication

Console examples for managing Azure infrastructure with the Azure Resource Manager SDK.

## Capabilities

- Create and inspect virtual machines and network interfaces.
- Create managed disks from VHDs or Azure Compute Gallery image versions.
- Create Compute Galleries, image definitions, and image versions.
- Build VM profiles for x64 and Arm64-compatible SKUs.
- Update virtual-network subnet and network-security-group resources.
- Reference Private DNS and other Azure resource-management packages.

The active workflow is selected in `Program.Main`; most operations are intentionally commented out so infrastructure-changing examples must be enabled explicitly.

## Authentication and configuration

The examples use `VisualStudioCredential` and expect an authenticated Azure developer identity with access to the selected subscription and resource groups. Replace placeholder subscription IDs, resource IDs, names, and locations before running.

VM examples may read:

- `AzureAdminUsername`
- `AzureAdminPassword`

Do not store credentials in source control.

## Run

Review the selected operation carefully, then run:

```powershell
dotnet run --project .\AzureConsoleApplication\AzureConsoleApplication\AzureConsoleApplication.csproj
```

These samples can create or modify billable Azure resources.
