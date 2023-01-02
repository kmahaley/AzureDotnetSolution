using DistributedCacheApplication.Models;
using DistributedCacheApplication.Services;
using Microsoft.AspNetCore.Mvc;

namespace DistributedCacheApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DistributedCacheController : ControllerBase
    {
        //private readonly ILogger<DistributedCacheController> logger;
        
        //private readonly ICacheService cacheService;


        //public DistributedCacheController(ILogger<DistributedCacheController> logger, ICacheService cacheService)
        //{
        //    this.logger = logger;
        //    this.cacheService = cacheService;
        //}

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetCacheValueAsync(int id)
        //{
        //    var existingEmployee = await cacheService.GetCacheAsync(id);
        //    if (existingEmployee == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(existingEmployee);
        //}

        //[HttpPost]
        //public async Task<IActionResult> SetCacheValueAsync([FromBody] Employee employee)
        //{
        //    if (employee == null)
        //    { 
        //        return BadRequest("Employee request data failed validation");
        //    }
        //    await cacheService.SetCacheAsync(employee.Id, employee);
        //    return Ok();
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteCacheValueAsync(int id)
        //{
        //    throw new NotImplementedException();
        //}

        //[HttpGet("allCacheData")]
        //public async Task<IActionResult> GetCacheValueAsync()
        //{
        //    throw new NotImplementedException();
        //}
    }
}