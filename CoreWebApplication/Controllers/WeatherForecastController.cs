using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration config;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfiguration configuration)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            config = configuration;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> GetAsync(CancellationToken token = default)
        {
            var x = config["KeyValue"];
            logger.LogInformation($"--------------{x}");
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