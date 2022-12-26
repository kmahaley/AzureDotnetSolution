using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SqlDbApplication.Models.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using SqlDbApplication.Services.Interface;
using SqlDbApplication.Exceptions;

namespace SqlDbApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointOfInterestController : ControllerBase
    {
        private readonly ILogger<PointOfInterestController> logger;
        private readonly IPointOfInterestService pointOfInterestService;

        public PointOfInterestController(ILogger<PointOfInterestController> logger, IPointOfInterestService pointOfInterestService)
        {
            this.logger = logger;
            this.pointOfInterestService = pointOfInterestService;
        }

        // GET: api/<CityController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetAllAsync()
        {
            var listOfCities = await pointOfInterestService.GetAllPointOfInterestsAsync();
            return Ok(listOfCities);
        }

        // GET api/<CityController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PointOfInterestDto>> GetAsync(int id)
        {
            try
            {
                var existingCity = await pointOfInterestService.GetPointOfInterestByIdAsync(id);
                return Ok(existingCity);
            }
            catch (SqlDbApplicationException ex)
            {
                return BadRequest();
            }
        }

        // POST api/<CityController>
        [HttpPost]
        public async Task<ActionResult<PointOfInterestDto>> PostAsync([FromBody] PointOfInterestDto pointOfInterest)
        {
            logger.LogInformation("Adding data.---");
            var savedCity = await pointOfInterestService.AddPointOfInterestAsync(pointOfInterest);
            return Ok(savedCity);
        }

        // PUT api/<CityController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<PointOfInterestDto>> PutAsync(int id, [FromBody] PointOfInterestDto city)
        {
            var updatedCity = await pointOfInterestService.UpdatePointOfInterestAsync(id, city);
            return Ok(updatedCity);
        }
    }
}
