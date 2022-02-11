using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SqlDbApplication.Repositories.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SqlDbApplication.Services
{
    public class FireAndForgetService : IFireAndForgetService
    {
        private readonly IServiceScopeFactory serviceScopeFactory;

        private readonly ILogger<FireAndForgetService> logger;

        public FireAndForgetService(IServiceScopeFactory serviceScopeFactory, ILogger<FireAndForgetService> logger)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            this.logger = logger;
        }

        public void ExecuteFireAndForgetJob(Func<IProductRepository, Task> jobFunction)
        {
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
                logger.LogError($"error if fire and forget {ex.Message}");
                throw;
            }



        }
    }
}
