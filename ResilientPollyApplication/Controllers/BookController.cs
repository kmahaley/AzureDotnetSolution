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

        public BookController(IEnumerable<IHttpService> httpServices, ILogger<BookController> logger)
        {
            this.logger = logger;
            pollyWrappedService = httpServices.First(s => "PollyWrappedService".Equals(s.GetServiceName()));
        }

        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            return await pollyWrappedService.TestHttpCallWithPollyBasedFramework();
        }
    }
}
