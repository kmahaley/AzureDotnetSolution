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