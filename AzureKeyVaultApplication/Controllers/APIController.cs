using AzureKeyVaultApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public IEnumerable<int> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5);
        }

        [HttpGet("keyvault")]
        public async Task<string> GetKeyVaultAsync()
        {
            var secretAsUser = await keyVaultService.GetSecretAsUserAsync();
            logger.LogInformation($"--------- user secret as User: {secretAsUser}");

            var secretAsClientSecret = await keyVaultService.GetSecretAsApplicationUsingClientSecretAsync();
            logger.LogInformation($"--------- user secret as AAD client app + client secret: {secretAsClientSecret}");

            var secretAsClientCertificate = await keyVaultService.GetSecretAsApplicationUsingClientCertificateAsync();
            logger.LogInformation($"--------- user secret as AAD client app + client certificate {secretAsClientCertificate}");

            // Will work when deployed in Azure resource eg. Webapps,VMSS etc.
            var secretAsmanagedIdentity = await keyVaultService.GetSecretAsApplicationUsingManagedIdentityAsync();
            logger.LogInformation($"--------- user secret ManagedIdentity {secretAsmanagedIdentity}");
            
            return secretAsUser; 
        }
    }
}
