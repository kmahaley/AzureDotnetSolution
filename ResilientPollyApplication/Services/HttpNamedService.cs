using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Timeout;
using ResilientPollyApplication.Constants;
using ResilientPollyApplication.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using UtilityLibrary.Constants;
using UtilityLibrary.PollyProject;

namespace ResilientPollyApplication.Services
{
    public class HttpNamedService : IHttpService
    {

        private readonly IHttpClientFactory httpClientFactory;

        private readonly HttpClient httpClient;

        private readonly ILogger<HttpNamedService> logger;

        private readonly IAsyncPolicy<HttpResponseMessage> policy1;

        private readonly IAsyncPolicy<HttpResponseMessage> policy2;

        public HttpNamedService(IHttpClientFactory httpClientFactory, ILogger<HttpNamedService> logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
            this.httpClient = httpClientFactory.CreateClient("transientpolicy");
            policy1 = NamedHttpClientBasedPolicy.CreateCircuitBreakerPolicy(durationOfTheBreak: TimeSpan.FromSeconds(30));
            policy2 = NamedHttpClientBasedPolicy.CreateCircuitBreakerPolicy(durationOfTheBreak: TimeSpan.FromSeconds(30));
        }

        public string GetServiceName()
        {
            return "HttpNamedService";
        }

        public async Task<List<string>> TestHttpCallWithPollyBasedFramework()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:8080/student/1");

            HttpResponseMessage response = null;

            var sw = Stopwatch.StartNew();
            try
            {
                var httpClient = this.httpClientFactory.CreateClient(RetryableConstants.PollyBasedNamedHttpClient);

                request.SetPolicyExecutionContext(CreatePolicyExecutionContext("TestHttpCallWithPollyBasedFramework"));
                response = await policy1.ExecuteAsync(() => httpClient.SendAsync(request));

                //response = await httpClient.SendAsync(request);
                logger.LogInformation($"-------- time taken for the complete request : {sw.ElapsedMilliseconds}ms");
            }
            catch(Exception ex)
            {
                logger.LogInformation($"-------- Exception : time taken for the complete request : {sw.ElapsedMilliseconds}ms");
                if(ex.GetType() == typeof(BrokenCircuitException) || ex.GetType() == typeof(TimeoutRejectedException))
                {
                    logger.LogError($"-------- { nameof(HttpNamedService)} {ex.GetType()}. Http call failed with Polly based exception.");
                }
                else
                {
                    logger.LogError(ex, $"-------- {nameof(HttpNamedService)} TestHttpCallWithPollyBasedFramework. Http call failed.");
                }
                throw;
            }

            return await ProcessResponse(response);
        }


        public async Task<List<string>> TestHttpCallWithPollyBasedFrameworkDuplicate()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:8080/student/2");

            HttpResponseMessage response = null;

            var sw = Stopwatch.StartNew();
            try
            {
                var httpClient = this.httpClientFactory.CreateClient(RetryableConstants.PollyBasedNamedHttpClient);

                request.SetPolicyExecutionContext(CreatePolicyExecutionContext("TestHttpCallWithPollyBasedFrameworkDuplicate"));
                response = await policy2.ExecuteAsync( () => httpClient.SendAsync(request));

                //response = await httpClient.SendAsync(request);
                logger.LogInformation($"-------- time taken for the complete request : {sw.ElapsedMilliseconds}ms");
            }
            catch(Exception ex)
            {
                logger.LogInformation($"-------- Exception : time taken for the complete request : {sw.ElapsedMilliseconds}ms");
                if(ex.GetType() == typeof(BrokenCircuitException) || ex.GetType() == typeof(TimeoutRejectedException))
                {
                    logger.LogError($"-------- { nameof(HttpNamedService)} {ex.GetType()}. Http call failed with Polly based exception.");
                }
                else
                {
                    logger.LogError(ex, $"-------- {nameof(HttpNamedService)} TestHttpCallWithPollyBasedFramework. Http call failed.");
                }
                throw;
            }

            return await ProcessResponse(response);
        }

        private Context CreatePolicyExecutionContext(string operationKey)
        {
            
            var context = new Context(operationKey, new Dictionary<string, object>
                {
                    { PollyConstant.LoggerKey, logger }
                });
            return context;
        }

        private async Task<List<string>> ProcessResponse(HttpResponseMessage response)
        {
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
                logger.LogError($"-------- {code}, failed to process http response.");

            }
            return students.Select(s => s.Name).ToList();
        }
    }
}
