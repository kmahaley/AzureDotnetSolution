using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using AzureKeyVaultApplication.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureKeyVaultApplication.Extensions
{
    public static class KeyVaultClient
    {
        public static IServiceCollection CreateKeyVaultClient(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();

            var keyvaultConfig = serviceProvider.GetRequiredService<IOptions<KeyVaultConfiguration>>();
            var keyVaultConfiguration = keyvaultConfig.Value;

            var options = new SecretClientOptions()
            {
                Retry =
                {
                     Delay= TimeSpan.FromSeconds(2),
                     MaxDelay = TimeSpan.FromSeconds(5),
                     MaxRetries = 5,
                     Mode = RetryMode.Exponential
                }
            };
            var credentialOptions = new DefaultAzureCredentialOptions
            {
                ExcludeAzureCliCredential = true,
                ExcludeInteractiveBrowserCredential = true,
                ExcludeSharedTokenCacheCredential = true,
                ExcludeVisualStudioCodeCredential = true,
                ExcludeVisualStudioCredential = true,

            };

            if(keyVaultConfiguration.ManagedIdentity)
            {
                credentialOptions.ExcludeEnvironmentCredential = true;
            }

            var keyVaultUrl = keyVaultConfiguration.Url;
            var client = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential(credentialOptions), options);
            services.AddSingleton(client);

            return services;
        }
    }
}
