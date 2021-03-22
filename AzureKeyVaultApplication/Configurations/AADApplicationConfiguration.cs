namespace AzureKeyVaultApplication.Configurations
{
    public class AADApplicationConfiguration
    {
        public string Name { get; set; }

        public string TenantId { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string Thumbprint { get; set; }
    }
}