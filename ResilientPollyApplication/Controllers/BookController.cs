using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ResilientPollyApplication.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResilientPollyApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly ILogger<BookController> logger;

        private readonly IHttpService pollyWrappedService;

        private readonly IHttpService httpTypedService;

        private readonly IHttpService httpNamedService;

        public BookController(IEnumerable<IHttpService> httpServices, ILogger<BookController> logger)
        {
            this.logger = logger;
            this.pollyWrappedService = httpServices.First(s => "PollyWrappedService".Equals(s.GetServiceName()));
            this.httpTypedService = httpServices.First(s => "HttpTypedService".Equals(s.GetServiceName()));
            this.httpNamedService = httpServices.First(s => "HttpNamedService".Equals(s.GetServiceName()));
        }

        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            logger.LogInformation("^^^^^^^^^^^^^^^^^^^^^ BookController call");
            return await httpTypedService.TestHttpCallWithPollyBasedFramework();
        }

        [HttpGet("named/1")]
        public async Task<IEnumerable<string>> GetNamedClient()
        {
            logger.LogInformation("^^^^^^^^^^^^^^^^^^^^^ BookController call");
            return await httpNamedService.TestHttpCallWithPollyBasedFramework();
        }

        [HttpGet("named/2")]
        public async Task<IEnumerable<string>> GetNamedClientDuplicate()
        {
            logger.LogInformation("^^^^^^^^^^^^^^^^^^^^^ BookController duplicate call");
            return await httpNamedService.TestHttpCallWithPollyBasedFrameworkDuplicate();
        }
    }
}
