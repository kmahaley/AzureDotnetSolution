using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureKeyVaultApplication.Configurations
{
    public class KeyVaultConfiguration
    {
        public string Url { get; set; }

        public string SecretName { get; set; }

        public bool ManagedIdentity { get; set; }

        public bool UserAssignedManagedIdentity { get; set; }

        public string UserAssignedManagedIdentityClientId { get; set; }


    }
}
