using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ResilientPollyApplication.Services
{
    public class PollyWrappedService : IHttpService
    {
        private readonly IHttpClientFactory httpClientFactory;

        private readonly ILogger<PollyWrappedService> logger;

        public PollyWrappedService(IHttpClientFactory httpClientFactory, ILogger<PollyWrappedService> logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
        }

        public Task TestHttpCallWithPollyBasedFramework()
        {
            throw new NotImplementedException();
        }
    }
}
