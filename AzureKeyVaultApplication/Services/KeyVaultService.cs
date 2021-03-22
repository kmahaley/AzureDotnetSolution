using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using AzureKeyVaultApplication.Configurations;
using AzureKeyVaultApplication.Utility;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace AzureKeyVaultApplication.Services
{
    public class KeyVaultService : IKeyVaultService
    {
        private readonly ILogger<KeyVaultService> logger;

        private readonly KeyVaultConfiguration keyVaultConfiguration;

        private readonly AADApplicationConfiguration applicationConfiguration;

        public KeyVaultService(ILogger<KeyVaultService> logger,
            IOptions<KeyVaultConfiguration> keyVaultConfiguration,
            IOptions<AADApplicationConfiguration> applicationConfiguration)
        {
            this.logger = logger;
            this.keyVaultConfiguration = keyVaultConfiguration.Value;
            this.applicationConfiguration = applicationConfiguration.Value;
        }

        public bool IsManagedIdentityEnabled()
        {
            return keyVaultConfiguration.ManagedIdentity;
        }

        public bool IsUserManagedIdentityEnabled()
        {
            return keyVaultConfiguration.UserAssignedManagedIdentity;
        }

        public async Task<string> GetSecretAsUserAsync()
        {
            logger.LogInformation("----- User Async");
            SecretClientOptions options = KeyVaultUtility.CreateSecretClientOptions();

            /*
             * Environment variables enabled. VS2019->application->properties->debug->Environment variable
             * AZURE_CLIENT_SECRET, AZURE_CLIENT_ID, AZURE_TENANT_ID
             * You can login in VS2019 with credentials having access to key vault. Add ExcludeEnvironmentCredential = true and
             * remove ExcludeVisualStudioCodeCredential
             */
            var credentialOptions = new DefaultAzureCredentialOptions
            {
                ExcludeAzureCliCredential = true,
                ExcludeInteractiveBrowserCredential = true,
                ExcludeSharedTokenCacheCredential = true,
                ExcludeEnvironmentCredential = true,
                ExcludeManagedIdentityCredential = true,
                ExcludeVisualStudioCodeCredential = true
            };
            var credentials = new DefaultAzureCredential(credentialOptions);

            var keyVault = keyVaultConfiguration.Url;
            var client = new SecretClient(new Uri(keyVault), credentials, options);

            KeyVaultSecret secret = null;
            try
            {
                secret = await client.GetSecretAsync(keyVaultConfiguration.SecretName);
                return secret.Value;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to get secret as user");
                throw;
            }
        }

        public async Task<string> GetSecretAsApplicationUsingClientSecretAsync()
        {
            logger.LogInformation("----- Client Secret Async");

            SecretClientOptions options = KeyVaultUtility.CreateSecretClientOptions();
            TokenCredentialOptions tokenOptions = KeyVaultUtility.CreateTokenCredentialOptions();

            var credential = new ClientSecretCredential(
                applicationConfiguration.TenantId,
                applicationConfiguration.ClientId,
                Environment.GetEnvironmentVariable("AZURE_CLIENT_SECRET"),//applicationConfiguration.ClientSecret
                tokenOptions);

            var keyVault = keyVaultConfiguration.Url;
            var client = new SecretClient(new Uri(keyVault), credential, options);

            KeyVaultSecret secret = null;
            try
            {
                secret = await client.GetSecretAsync(keyVaultConfiguration.SecretName);
                return secret.Value;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to get secret as client secret");
                throw;
            }
        }

        public async Task<string> GetSecretAsApplicationUsingClientCertificateAsync()
        {
            logger.LogInformation("----- Client Certificate Async");

            SecretClientOptions options = KeyVaultUtility.CreateSecretClientOptions();
            var keyVault = keyVaultConfiguration.Url;

            var credential = new ClientCertificateCredential(
                applicationConfiguration.TenantId,
                applicationConfiguration.ClientId,
                KeyVaultUtility.GetCertificate(applicationConfiguration.Thumbprint));

            var client = new SecretClient(new Uri(keyVault), credential, options);

            KeyVaultSecret secret = null;
            try
            {
                secret = await client.GetSecretAsync(keyVaultConfiguration.SecretName);
                return secret.Value;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to get secret as client sertificate");
                throw;
            }
        }

        public async Task<string> GetSecretAsApplicationUsingManagedIdentityAsync()
        {
            logger.LogInformation("----- System Assigned Managed Identity");

            SecretClientOptions options = KeyVaultUtility.CreateSecretClientOptions();

            var credentialOptions = new DefaultAzureCredentialOptions
            {
                ExcludeAzureCliCredential = true,
                ExcludeInteractiveBrowserCredential = true,
                ExcludeSharedTokenCacheCredential = true,
                ExcludeEnvironmentCredential = true,
                ExcludeVisualStudioCredential = true,
                ExcludeVisualStudioCodeCredential = true
            };
            var credentials = new DefaultAzureCredential(credentialOptions);

            var keyVault = keyVaultConfiguration.Url;
            var client = new SecretClient(new Uri(keyVault), credentials, options);

            KeyVaultSecret secret = null;
            try
            {
                secret = await client.GetSecretAsync(keyVaultConfiguration.SecretName);
                return secret.Value;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to get secret as managed identity");
                throw;
            }
        }

        public async Task<string> GetSecretAsApplicationUsingUserManagedIdentityAsync()
        {
            logger.LogInformation("----- User Assigned Managed Identity");

            SecretClientOptions options = KeyVaultUtility.CreateSecretClientOptions();

            var credentials = new ManagedIdentityCredential(keyVaultConfiguration.UserAssignedManagedIdentityClientId);

            var keyVault = keyVaultConfiguration.Url;
            var client = new SecretClient(new Uri(keyVault), credentials, options);

            KeyVaultSecret secret = null;
            try
            {
                secret = await client.GetSecretAsync(keyVaultConfiguration.SecretName);
                return secret.Value;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to get secret as managed identity");
                throw;
            }
        }
    }

    public interface IKeyVaultService
    {
        bool IsUserManagedIdentityEnabled();

        bool IsManagedIdentityEnabled();

        Task<string> GetSecretAsUserAsync();

        Task<string> GetSecretAsApplicationUsingClientSecretAsync();

        Task<string> GetSecretAsApplicationUsingClientCertificateAsync();

        Task<string> GetSecretAsApplicationUsingManagedIdentityAsync();

        Task<string> GetSecretAsApplicationUsingUserManagedIdentityAsync();
    }
}