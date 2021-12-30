using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace NoHttpWorkloadApplication.Services
{
    public class BookService : IHostedService
    {
        private readonly ILogger logger;

        public BookService(ILogger<BookService> logger, IHostApplicationLifetime appLifetime)
        {
            this.logger = logger;

            appLifetime.ApplicationStarted.Register(OnStarted);
            appLifetime.ApplicationStopping.Register(OnStopping);
            appLifetime.ApplicationStopped.Register(OnStopped);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("1. StartAsync has been called.");

            return Task.CompletedTask;
        }

        private void OnStarted()
        {
            logger.LogInformation("2. OnStarted has been called.");
        }

        private void OnStopping()
        {
            logger.LogInformation("3. OnStopping has been called.");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("4. StopAsync has been called.");

            return Task.CompletedTask;
        }

        private void OnStopped()
        {
            logger.LogInformation("5. OnStopped has been called.");
        }
    }
}