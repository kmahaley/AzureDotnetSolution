using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ResilientPollyApplication.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResilientPollyApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LibraryController : ControllerBase
    {
        private readonly ILogger<LibraryController> logger;

        private readonly IHttpService pollyWrappedService;

        private readonly IHttpService httpTypedService;

        private readonly IHttpService httpNamedService;

        public LibraryController(IEnumerable<IHttpService> httpServices, ILogger<LibraryController> logger)
        {
            this.logger = logger;
            this.pollyWrappedService = httpServices.First(s => "PollyWrappedService".Equals(s.GetServiceName()));
            this.httpTypedService = httpServices.First(s => "HttpTypedService".Equals(s.GetServiceName()));
            this.httpNamedService = httpServices.First(s => "HttpNamedService".Equals(s.GetServiceName()));
        }

        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            logger.LogInformation("^^^^^^^^^^^^^^^^^^^^^ LibraryController call");
            return await httpTypedService.TestHttpCallWithPollyBasedFramework();
        }

        [HttpGet("named")]
        public async Task<IEnumerable<string>> GetNamedClient()
        {
            logger.LogInformation("^^^^^^^^^^^^^^^^^^^^^ LibraryController call");
            return await httpNamedService.TestHttpCallWithPollyBasedFramework();
        }
    }
}