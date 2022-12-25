using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SqlDbApplication.Models.Dtos;
using SqlDbApplication.Models.Sql;
using SqlDbApplication.Services.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SqlDbApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ILogger<CityController> logger;
        private readonly ICityService cityService;

        public CityController(ILogger<CityController> logger, ICityService cityService)
        {
            this.logger = logger;
            this.cityService = cityService;
        }

        // GET: api/<CityController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityDto>>> GetAllAsync()
        {
            var listOfCities = await cityService.GetAllCitiesAsync();
            return Ok(listOfCities);
        }

        // GET api/<CityController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CityDto>> GetAsync(int id)
        {
            try
            {
                var existingCity = await cityService.GetCityByIdAsync(id);
                return Ok(existingCity);
            }
            catch (ArgumentException ex)
            {
                return BadRequest();
            }
        }

        // POST api/<CityController>
        [HttpPost]
        public async Task<ActionResult<CityDto>> PostAsync([FromBody] CityDto city)
        {
            logger.LogInformation("Adding data.---");
            var savedCity = await cityService.AddCityAsync(city);
            return Ok(savedCity);
        }

        // PUT api/<CityController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<CityDto>> PutAsync(int id, [FromBody] CityDto city)
        {
            var updatedCity = await cityService.UpdateCityAsync(id, city);
            return Ok(updatedCity);
        }

        // DELETE api/<CityController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CityDto>> DeleteAsync(int id)
        {
            var deletedCity = await cityService.DeleteCityByIdAsync(id);
            return Ok(deletedCity);
        }
    }
}
