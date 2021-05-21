using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CoreWebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> logger;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly HttpClient httpClient;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IHttpClientFactory httpClientFactory)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.httpClient = httpClientFactory.CreateClient("named");
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> GetAsync(CancellationToken token = default)
        {
            var sw = Stopwatch.StartNew();
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:8080/student/1");

            HttpResponseMessage response = null;

            try
            {
                request.SetPolicyExecutionContext(new Context("WeatherForecastGet"));
                response = await this.httpClient.SendAsync(request, token);
                logger.LogInformation($"response: {response.IsSuccessStatusCode}");
            }
            catch (Exception ex)
            {
                logger.LogError($"Request failed {ex.GetType()} : {ex.Message}");
                throw new ArgumentException($"We don't offer a weather forecast for {ex.GetType()} ");
            }
            
            logger.LogInformation($"-------- time taken for the complete request : {sw.ElapsedMilliseconds}ms");
            return new WeatherForecast[1]
            {
                new WeatherForecast
                { 
                    Date = new DateTime(),
                    TemperatureC =72,
                    Summary = "sunny"
                }
            };
        }
    }
}