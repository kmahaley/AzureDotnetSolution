using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SqlDbApplication.Repositories.Sql.Interface;
using SqlDbApplication.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SqlDbApplication.Services
{
    /// <summary>
    /// FireAndForgetService service is injected with IServiceScopeFactory. We can inject IServiceProvider
    /// too. Use any method to create scope.
    /// </summary>
    public class FireAndForgetService : IFireAndForgetService
    {
        private readonly IServiceScopeFactory serviceScopeFactory;

        private readonly ILogger<FireAndForgetService> logger;

        public FireAndForgetService(IServiceScopeFactory serviceScopeFactory, ILogger<FireAndForgetService> logger)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            this.logger = logger;
        }

        /// <summary>
        /// serviceScopeFactory create a scope for IProductRepository and updates database
        /// </summary>
        public void ExecuteFireAndForgetJob(Func<IProductRepository, Task> jobFunction)
        {
            logger.LogInformation("--- firing fire and forget job");
            try
            {
                Task.Run(async () =>
                {
                    using var scope = serviceScopeFactory.CreateScope();
                    var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
                    await jobFunction(productRepository);
                });
            }
            catch(Exception ex)
            {
                logger.LogError($"--- error if fire and forget.\n {ex.Message}");
                throw;
            }



        }
    }
}
