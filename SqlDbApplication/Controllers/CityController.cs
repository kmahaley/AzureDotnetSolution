using Microsoft.AspNetCore.Http;
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
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private const int MaxPageSize = 10;
        private readonly ILogger<CityController> logger;
        private readonly ICityService cityService;

        public CityController(ILogger<CityController> logger, ICityService cityService)
        {
            this.logger = logger;
            this.cityService = cityService;
        }

        /// <summary>
        /// Get all cities present in the application
        /// </summary>
        /// <param name="includePoints">should you include point of interests</param>
        /// <returns>List of all Cities with point of interests</returns>
        // GET: api/<CityController>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityDto>>> GetAllAsync([FromQuery] bool? includePoints)
        {
            var listOfCities = await cityService.GetAllCitiesAsync(includePoints);
            return Ok(listOfCities);
        }

        /// <summary>
        /// Get city data by city id
        /// </summary>
        /// <param name="id">city identifier</param>
        /// <param name="includePoints">should you include point of interests</param>
        /// <returns>return city data for city id provided</returns>
        // GET api/<CityController>/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{id}")]
        public async Task<ActionResult<CityDto>> GetAsync(int id, [FromQuery] bool? includePoints)
        {
            try
            {
                var existingCity = await cityService.GetCityByIdAsync(id, includePoints);
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

        /// <summary>
        /// Filter based on name of the city. Filter need exact matching record.
        /// </summary>
        /// <param name="name">name of the city used for filtering</param>
        /// <param name="includePoints">should include dependent point of interest</param>
        /// <returns>list of cities matching filter.</returns>
        [HttpGet("/filter")]
        public async Task<ActionResult<IEnumerable<CityDto>>> GetAllCitiesFilteredUsingNameAsync(
            [FromQuery] string? name,
            [FromQuery] bool includePoints = false)
        {
            var listOfCities = await cityService.GetAllCitiesFilteredUsingNameAsync(name, includePoints);
            return Ok(listOfCities);
        }

        /// <summary>
        /// Added query params to filter on city name and search on name and description
        /// once filter is applied using name then search is applied on returned filtered.
        /// searchQuery gives result containg the user provided string
        /// </summary>
        /// <param name="name">name of the city used for filerting</param>
        /// <param name="searchQuery">name of search query checked in name and description of the city</param>
        /// <param name="includePoints">should include dependent point of interest</param>
        /// <returns>list of cities matching filter and search.</returns>
        [HttpGet("/search")]
        public async Task<ActionResult<IEnumerable<CityDto>>> GetAllCitiesUsingSearchAsync(
            [FromQuery] string? name,
            [FromQuery] string? searchQuery,
            [FromQuery] bool includePoints = false)
        {
            var listOfCities = await cityService.GetAllCitiesUsingSearchAsync(name, searchQuery, includePoints);
            return Ok(listOfCities);
        }

        /// <summary>
        /// Added query params to filter on city name and search on name && description
        /// once filter is applied using name then search is applied on returned filtered.
        /// searchQuery gives result containg the user provided string
        /// </summary>
        /// <param name="name">name of the city used for filerting</param>
        /// <param name="searchQuery">name of search query checked in name and description of the city</param>
        /// <param name="includePoints">should include dependent point of interest</param>
        /// <param name="pageNumber">skip number of results. value starts from 1.</param>
        /// <param name="pageSize">how many cities returned in the call</param>
        /// <returns>list of cities matching filter.</returns>
        [HttpGet("/pagedSearch")]
        public async Task<ActionResult<IEnumerable<CityDto>>> GetAllCitiesUsingSearchAndPaginationAsync(
            [FromQuery] string? name,
            [FromQuery] string? searchQuery,
            [FromQuery] bool includePoints = false,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5)
        {
            if (pageSize > MaxPageSize)
            {
                pageSize = MaxPageSize;
            }
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }
            var listOfCities = await cityService.GetAllCitiesUsingSearchAndPaginationAsync(
                name,
                searchQuery,
                includePoints,
                pageNumber,
                pageSize);
            return Ok(listOfCities);
        }

        /// <summary>
        /// Added query params to filter on city name and search on name && description
        /// once filter is applied using name then search is applied on returned filtered.
        /// searchQuery gives result containg the user provided string
        /// </summary>
        /// <param name="name">name of the city used for filerting</param>
        /// <param name="searchQuery">name of search query checked in name and description of the city</param>
        /// <param name="includePoints">should include dependent point of interest</param>
        /// <param name="pageNumber">skip number of results. value starts from 1.</param>
        /// <param name="pageSize">how many cities returned in the call</param>
        /// <returns>list of cities matching filter.</returns>
        [HttpGet("/searchWithPagination")]
        public async Task<ActionResult<CityPageDto>> GetAllCitiesWithPaginationMetdadataAsync(
            [FromQuery] string? name,
            [FromQuery] string? searchQuery,
            [FromQuery] bool includePoints = false,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5)
        {
            if (pageSize > MaxPageSize)
            {
                pageSize = MaxPageSize;
            }
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }
            var cityPageDto = await cityService.GetAllCitiesWithPaginationMetdadataAsync(
                name,
                searchQuery,
                includePoints,
                pageNumber,
                pageSize);

            return Ok(cityPageDto);
        }
    }
}
