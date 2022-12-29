namespace SqlDbApplication.Configurations
{
    public class AuthenticationConfiguration
    {
        public string SecretForKey { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }
    }
}
