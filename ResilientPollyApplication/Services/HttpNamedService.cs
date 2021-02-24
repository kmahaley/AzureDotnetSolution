using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ResilientPollyApplication.Services
{
    public class HttpNamedService : IHttpService
    {

        private readonly IHttpClientFactory httpClientFactory;

        private readonly ILogger<HttpNamedService> logger;

        public HttpNamedService(IHttpClientFactory httpClientFactory, ILogger<HttpNamedService> logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
        }

        public string GetServiceName()
        {
            return "HttpNamedService";
        }

        public async Task<List<string>> TestHttpCallWithPollyBasedFramework()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5000/student/mock");
            HttpClient client;
            HttpResponseMessage response = null;

            try
            {

                client = httpClientFactory.CreateClient();
               
            }
            catch(Exception ex)
            {
                logger.LogError(ex, $"{nameof(HttpNamedService)} TestHttpCallWithPollyBasedFramework. Http call failed");
            }

            if(response != null && response.IsSuccessStatusCode)
            {
                using var res = await response.Content.ReadAsStreamAsync();
                IEnumerable<string> list = await JsonSerializer.DeserializeAsync<IEnumerable<string>>(res);
            }
            else
            {
                logger.LogError($"{(int)response.StatusCode}, failed to process http response.");
            }
            return new List<string>();
        }
    }
}
