using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoreWebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> logger;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly HttpClient httpClient;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IHttpClientFactory httpClientFactory)
        {
            logger = logger;
            httpClient = httpClientFactory.CreateClient("named");
            
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> GetAsync()
        {
            var sw = Stopwatch.StartNew();
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:8080/student/1");

            HttpResponseMessage response = null;

            try
            {
                request.SetPolicyExecutionContext(new Context("WeatherForecastGet"));
                response = await this.httpClient.SendAsync(request);
                logger.LogInformation($"response: {response.IsSuccessStatusCode}" );
            }
            catch(Exception ex)
            {
                
                throw;
            }
            logger.LogInformation($"-------- time taken for the complete request : {sw.ElapsedMilliseconds}ms");

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
