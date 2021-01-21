using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace AzureKeyVaultApplication.Utility
{
    public static class KeyVaultUtility
    {
        public static TokenCredentialOptions CreateTokenCredentialOptions()
        {
            return new TokenCredentialOptions()
            {
                Retry =
                {
                     Delay= TimeSpan.FromSeconds(2),
                     MaxDelay = TimeSpan.FromSeconds(16),
                     MaxRetries = 5,
                     Mode = RetryMode.Exponential
                }
            };
        }

        public static SecretClientOptions CreateSecretClientOptions()
        {
            return new SecretClientOptions()
            {
                Retry =
                {
                     Delay= TimeSpan.FromSeconds(2),
                     MaxDelay = TimeSpan.FromSeconds(16),
                     MaxRetries = 5,
                     Mode = RetryMode.Exponential
                }
            };
        }

        /// <summary>
        /// Get certificate by thumbprint.
        /// </summary>
        /// <param name="thumbprint">Certificate thumbprint.</param>
        /// <returns>X509 Certificate.</returns>
        public static X509Certificate2 GetCertificate(string thumbprint)
        {
            if(string.IsNullOrEmpty(thumbprint))
            {
                throw new ArgumentNullException(nameof(thumbprint));
            }

            X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            try
            {
                store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                X509Certificate2Collection certCollection = store.Certificates.Find(
                    X509FindType.FindByThumbprint, thumbprint, false);

                if(certCollection.Count != 1)
                {
                    throw new ArgumentException($"Can't find unique certificate with a given thumbprint: {thumbprint}. Store: {StoreLocation.LocalMachine}");
                }

                return certCollection[0];
            }
            finally
            {
                store.Close();
            }
        }
    }
}
