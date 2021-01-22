 AzureKeyVaultApplication

## Build

  - `git clone https://github.com/kmahaley/AzureDotnetSolution.git`
  - cd into AzureDotnetSolution -> AzureKeyVaultApplication
  - run following commands
    - `dotnet build`
    - `dotnet run`
    - Using postman try Get:`https://localhost:5001/api` , make sure you get `service running` response.

## How to setup Azure key vault

  - Login to `portal.azure.com` -> select subcription and resource group
  - Create keyvault. Keep note of [keyvaultName] and [keyVaultUrl]
  - Goto `Secrets` tab and add secret [secretName], enter value [secretValue]
  - Goto `Access policy`, make sure you have permissions to key, secret and certificate. If not select you name and  type of permission you want

## How to create Azure Active directory application registration with client secret

  - Goto Azure portal ->Azure active directory ->App registration ->new registartion -> provide name [appRegistartionName] and select single tenant -> create
  - Goto overview of app registration, make note of [tenantId] and [clientId]
  - Goto `Certificate & Secret` ->Add new client secret -> make note of [clientSecret] value

## How to create Azure Active directory application registration with client certificate

  - Create self signed certificate. Using `makecert` https://docs.microsoft.com/en-us/windows/win32/seccrypto/makecert
    - Create directory `certificate` and run below commands
    - `makecert -sv privateKey.pvk -n "cn=serviceFabricToKeyVaultAccess" serviceFabricToKeyVaultAccess.cer -b 12/30/2010 -e 12/29/2021 -r`
    - provide password [certificatePassword]
    - Create `.pfx` file using below command. details https://docs.microsoft.com/en-us/windows-hardware/drivers/devtest/pvk2pfx
    - `pvk2pfx -pvk privateKey.pvk -spc serviceFabricToKeyVaultAccess.cer -pfx serviceFabricToKeyVaultAccess.pfx -pi [certificatePassword]`
    - you can obtain thumbprint using powershell command `Get-PfxCertificate "[path]\serviceFabricToKeyVaultAccess.pfx"`
    - Install certificate on your machine. Double click and select .pfx -> install for local machine 
    
  - Goto Azure portal ->Azure active directory ->App registration ->new registartion -> provide name [appRegistartionName] and select single tenant -> create
  - Goto overview of app registration, make note of [tenantId] and [clientId]
  - Goto `Certificate & Secret` ->upload certificate -> select and add .cer file we created inprevious step ->make note of [clientCertificateThumbprint] value
  
***Installing certificate on Azure resource***

  - Install certificates on Azure Virtual machine scale set(VMSS)
    - Upload certificate in Azure keyvault. Goto Azure portal ->[keyvaultName]-> Certificate section->Import `.pfx` private key -> save
    - make note of secret identifier [certificateSecretIdentifier] 
    - example: https://[keyVaultUrl]/secrets/[certificateName]/[version]
    - update VMSS ARM template.
    - https://resources.azure.com/ -> select subscription -> resource group -> providers -> microsoft.compute -> VMSS name -> Put call to update VMSS
    - Add below mentioned json in ARM template VMSS -> virtualMachineProfile -> osProfile -> secrets -> vaultCertificates
    - After Put call make sure ARM template updated and VMSS are in updating state

```
{
"certificateUrl": "[certificateSecretIdentifier]",
"certificateStore": "My"
}
```

## Access Azure key vault using dotnet core

There are 4 ways to access Azure key vault secrets. You can make code behave as following.

#### Using user credentials

 User [ABC] has access to keyvault. User can use Visual studo, visual studio code as credentials to access key vault.
 Check `KeyVaultService.GetSecretAsUserAsync` method
 

#### Using Azure Active Directory(AAD) application with client secret

 If you want to make code independent of user login, use AAD application registration approach.
 
 Refer to section <b>How to create Azure Active directory application registration with client secret</b>`

  - You have [tenantId], [clientId] and [clientSecret] value.
  - Goto azure portal -> select [keyvaultName] -> Access policy -> Add access policy -> select template -> select principal as [appRegistartionName] -> save

 Now you [appRegistartionName] has access to [keyvaultName].

 Check `KeyVaultService.GetSecretAsApplicationUsingClientSecretAsync` method
 

#### Using Azure Active Directory(AAD) application with Client certificate

 If you want to make code independent of user login, use AAD application registration approach.
 Having [clientSecret] is bad coding practice. We can use certificate approach.
 
 Refer to section <b>How to create Azure Active directory application registration with client certificate</b> 

  - You have [tenantId], [clientId] and [clientCertificateThumbprint] value.
  - Goto azure portal -> select [keyvaultName] -> Access policy -> Add access policy -> select template -> select principal as [appRegistartionName] -> save
  
 Now you [appRegistartionName] has access to [keyvaultName]. 

 Check `KeyVaultService.GetSecretAsApplicationUsingClientCertificateAsync` method

#### Using Azure Managed Identity of Azure Resources [Best approach]

##### System Created Managed identity

 Having certificates, installation and renewal is tiring process. Azure provides an approach of enabling managed identity for Azure resource.
 Managed Identity creates an AAD application registration. All you need is give access to this app registration in key vault using access policy
 Now you [appRegistartionName] has access to [keyvaultName].

 In this example we will give Azure Webapp system assigned managed identity

  - Goto Azure portal ->Select Webapp resource -> Identity -> enable system assigned -> save
  - Goto key vault -> Add Access policy -> select permission -> select service principle as above Azure resource name -> save
  
 you can see that Azure created AAD application for Webapp resource in Azure portal -> AAD

 Managed identity works only when an Azure resource tries to access another Azure resource. https://docs.microsoft.com/en-us/azure/active-directory/managed-identities-azure-resources/

 Check `KeyVaultService.GetSecretAsApplicationUsingManagedIdentityAsync` method

***if you remove system assigned managed identity from azure resource then AAD application is deleted. you will loose access to key vault***

##### UserCreated Managed identity

  - Goto Azure portal -> Managed Identity-> Create user assigned managed Identity->make note of [UserAssignedManagedIdentityName] and client id of that identity [ClientIdOfUserAssignedManagedIdentityName]
  - Associate [UserAssignedManagedIdentityName] to Azure webapp resource 
  - Goto Azure portal -> Select Webapp resource -> Identity -> user assigned -> select [UserAssignedManagedIdentityName] -> save
  - Goto key vault -> Add Access policy -> select permission -> select service principle as [UserAssignedManagedIdentityName] -> save

 Check `KeyVaultService.GetSecretAsApplicationUsingUserManagedIdentityAsync` method