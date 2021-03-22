using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ResilientPollyApplication.Services
{
    public class PollyWrappedService : IHttpService
    {
        private readonly ILogger<PollyWrappedService> logger;

        public PollyWrappedService(ILogger<PollyWrappedService> logger)
        {
            this.logger = logger;
        }

        public string GetServiceName()
        {
            return "PollyWrappedService";
        }

        public async Task<List<string>> TestHttpCallWithPollyBasedFramework()
        {
            await Task.Delay(1000);
            throw new NotImplementedException();
        }

        public Task<List<string>> TestHttpCallWithPollyBasedFrameworkDuplicate()
        {
            throw new NotImplementedException();
        }
    }
}