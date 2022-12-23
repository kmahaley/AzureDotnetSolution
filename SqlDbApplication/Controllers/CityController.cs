using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SqlDbApplication.Models.Sql;
using SqlDbApplication.Services.Interface;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<City>>> GetAllAsync()
        {
            var list = await cityService.GetAllCitiesAsync();
            return Ok(list);
        }
    }
}
