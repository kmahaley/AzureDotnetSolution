using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ResilientPollyApplication.Services
{
    public class HttpTypedService : IHttpService
    {

        private readonly HttpClient httpClient;

        private readonly ILogger<HttpTypedService> logger;

        public HttpTypedService(HttpClient httpClient, ILogger<HttpTypedService> logger)
        {
            httpClient.BaseAddress = new Uri("http://localhost:5000/");
            
            this.httpClient = httpClient;
            this.logger = logger;
        }

        public string GetServiceName()
        {
            return "HttpTypedService";
        }

        public async Task<List<string>> TestHttpCallWithPollyBasedFramework()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "student/mock");
            HttpResponseMessage response = null;

            try
            {
                response = await httpClient.SendAsync(request);


            }
            catch(Exception ex)
            {
                logger.LogError(ex, $"{nameof(HttpTypedService)} TestHttpCallWithPollyBasedFramework. Http call failed.");
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
