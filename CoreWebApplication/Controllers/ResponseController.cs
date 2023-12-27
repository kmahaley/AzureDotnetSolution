﻿using CoreWebApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResponseController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<ResponseController> logger;

        public ResponseController(ILogger<ResponseController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> Get()
        {

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            _ = Task.Delay(1000);
            Random random = new Random(id);
            if (random.Next(0, 101) >= id)
            {
                logger.LogError("Failed, generated http 500");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            logger.LogInformation("Success, generated http 200");
            return Ok();
        }
    }
}
