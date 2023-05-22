using CoreWebApplication.Repositories;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoreWebApplication.Services
{
    public class BackgroundShortRunningService : IHostedService
    {
        private CancellationTokenSource cancellationTokenSource;

        private readonly ILogger logger;

        private readonly IServiceProvider services;

        private readonly IRepository repository;

        public BackgroundShortRunningService(IRepository repository, ILogger<BackgroundShortRunningService> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task StartAsync(CancellationToken startToken)
        {
            logger.LogInformation($"started short service. {logger == null}, {repository == null}");
            cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(startToken);

            for (int i = 0; i < 10; i++)
            {
                if (cancellationTokenSource.IsCancellationRequested)
                {
                    logger.LogError("------------------------- cancellationTokenSource stop requested");
                    return;
                }

                logger.LogInformation($"{i}");
                await Task.Delay(2000);
            }

            //return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogError("------------------------- short Service stop requested");
            cancellationTokenSource.Cancel();
            //cancellationToken.ThrowIfCancellationRequested();
            return Task.CompletedTask;
        }
    }
}
