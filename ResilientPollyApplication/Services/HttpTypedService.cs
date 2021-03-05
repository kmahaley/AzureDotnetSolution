using Microsoft.Extensions.Logging;
using Polly.CircuitBreaker;
using Polly.Timeout;
using ResilientPollyApplication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

            this.httpClient = httpClient;
            this.logger = logger;
        }

        public string GetServiceName()
        {
            return "HttpTypedService";
        }

        public async Task<List<string>> TestHttpCallWithPollyBasedFramework()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:8080/student");
            
            HttpResponseMessage response = null;

            try
            {
                response = await httpClient.SendAsync(request);
            }
            catch(Exception ex)
            {
                if(ex.GetType() == typeof(BrokenCircuitException) || ex.GetType() == typeof(TimeoutRejectedException))
                {
                    logger.LogError($"####### {ex.GetType()}. Http call failed with Polly based exception.");
                }
                else
                {
                    logger.LogError(ex, $"!!!!!!!!!!! {nameof(HttpTypedService)} TestHttpCallWithPollyBasedFramework. Http call failed.");
                }
                //throw;
            }

            IEnumerable<Student> students = new List<Student>();
            if(response != null && response.IsSuccessStatusCode)
            {
                using var res = await response.Content.ReadAsStreamAsync();
                
                var properties = new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                };
                students = await JsonSerializer.DeserializeAsync<IEnumerable<Student>>(res, properties);
            }
            else
            {
                int code = (response != null) ? (int)response.StatusCode : 0;
                logger.LogError($" ---------------------> {code}, failed to process http response.");
                
            }
            return students.Select(s => s.Name).ToList();
        }

        public Task<List<string>> TestHttpCallWithPollyBasedFrameworkDuplicate()
        {
            throw new NotImplementedException();
        }
    }
}
