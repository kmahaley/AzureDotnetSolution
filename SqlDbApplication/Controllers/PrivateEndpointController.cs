using System.Threading.Tasks;
using System.Threading;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using SqlDbApplication.Models.Dtos;
using SqlDbApplication.Repositories.Sql.Interface;
using SqlDbApplication.Services;
using SqlDbApplication.Services.Interface;
using System;

namespace SqlDbApplication.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class PrivateEndpointController : Controller
    {
        private readonly ILogger<PrivateEndpointController> logger;
        private readonly IPrivateEndpointRepository repository;

        public PrivateEndpointController(
            ILogger<PrivateEndpointController> logger,
            IPrivateEndpointRepository repository)
        {
            this.logger = logger;
            this.repository = repository;
        }

        // GET api/<PrivateEndpointController>
        [HttpPost]
        public async Task<ActionResult> PostAsync(
            [FromBody] PrivateEndpointRequest request,
            CancellationToken cancellationToken = default)
        {
            logger.LogInformation("creating PE");
            await repository.AddPrivateEndpointAsync(request, cancellationToken);
            return Ok(request);
        }

        // GET api/<PrivateEndpointController>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            logger.LogInformation("get PE.");
            var pe = await repository.GetPrivateEndpointAsync(id, cancellationToken);
            return Ok(pe);
        }

        // DELETE api/<PrivateEndpointController>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            logger.LogInformation("deleting PE concurrency issue.---");
            await repository.DeletePrivateEndpointAsync(id, cancellationToken);
            return Ok();
        }
    }
}
