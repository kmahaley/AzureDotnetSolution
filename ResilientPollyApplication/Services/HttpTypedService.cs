using Microsoft.Extensions.Logging;
using ResilientPollyApplication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web.Http;

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
                logger.LogError(ex, $"{nameof(HttpTypedService)} TestHttpCallWithPollyBasedFramework. Http call failed.");
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
                int code = response != null ? (int)response.StatusCode : 0;
                logger.LogError($" --------------------->    {code}, failed to process http response.");
                
            }
            return students.Select(s => s.Name).ToList();
        }
    }
}
