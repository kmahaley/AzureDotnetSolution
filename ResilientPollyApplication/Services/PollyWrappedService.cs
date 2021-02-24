using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ResilientPollyApplication.Model;
using ResilientPollyApplication.Polly;
using ResilientPollyApplication.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Polly;
using Polly.Retry;
using System;

namespace ResilientPollyApplication.Services
{
    public class PollyWrappedService : IHttpService
    {
        private readonly IHttpClientFactory httpClientFactory;

        private readonly ILogger<PollyWrappedService> logger;

        private readonly HttpClient client;

        public PollyWrappedService(IHttpClientFactory httpClientFactory, ILogger<PollyWrappedService> logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
            this.client = httpClientFactory.CreateClient("PollyWrappedService");
        }

        public string GetServiceName()
        { 
            return "PollyWrappedService";  
        }

        public async Task<List<string>> TestHttpCallWithPollyBasedFramework()
        {
            throw new NotImplementedException();
        }

        
    }
}
