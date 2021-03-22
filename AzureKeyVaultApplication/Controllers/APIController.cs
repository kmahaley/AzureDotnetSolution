using AzureKeyVaultApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace AzureKeyVaultApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class APIController : ControllerBase
    {
        private readonly ILogger<APIController> logger;

        private readonly IKeyVaultService keyVaultService;

        public APIController(ILogger<APIController> logger, IKeyVaultService keyVaultService)
        {
            this.logger = logger;
            this.keyVaultService = keyVaultService;
        }

        [HttpGet]
        public string Get()
        {
            return "service running...";
        }

        [HttpGet("keyvault")]
        public async Task<string> GetKeyVaultAsync()
        {
            string secretValue = "";
            var isSystemManagedIdentity = keyVaultService.IsManagedIdentityEnabled();
            var isUserAssignedManagedIdentity = keyVaultService.IsUserManagedIdentityEnabled();

            if (isSystemManagedIdentity)
            {
                // Will work when deployed in Azure resource eg. Webapps,VMSS etc.
                var secretAsmanagedIdentity = await keyVaultService.GetSecretAsApplicationUsingManagedIdentityAsync();
                logger.LogInformation($"--------- user secret as System Assigned ManagedIdentity {secretAsmanagedIdentity}");
                secretValue = $"System Managed identity: {isSystemManagedIdentity}, secret: {secretAsmanagedIdentity}";
            }
            else if (isUserAssignedManagedIdentity)
            {
                var secretAsUsermanagedIdentity = await keyVaultService.GetSecretAsApplicationUsingUserManagedIdentityAsync();
                logger.LogInformation($"--------- user secret as User Assigned ManagedIdentity {secretAsUsermanagedIdentity}");
                secretValue = $"User Managed identity: {isUserAssignedManagedIdentity}, secret: {secretAsUsermanagedIdentity}";
            }
            else
            {
                var secretAsUser = await keyVaultService.GetSecretAsUserAsync();
                logger.LogInformation($"--------- user secret as User: {secretAsUser}");

                var secretAsClientSecret = await keyVaultService.GetSecretAsApplicationUsingClientSecretAsync();
                logger.LogInformation($"--------- user secret as AAD client app + client secret: {secretAsClientSecret}");

                var secretAsClientCertificate = await keyVaultService.GetSecretAsApplicationUsingClientCertificateAsync();
                logger.LogInformation($"--------- user secret as AAD client app + client certificate {secretAsClientCertificate}");

                secretValue = $"Managed identity: {isSystemManagedIdentity}, secret: {secretAsClientSecret}";
            }

            return secretValue;
        }
    }
}