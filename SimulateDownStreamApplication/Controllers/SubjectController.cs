using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimulateDownStreamApplication.Model;
using SimulateDownStreamApplication.Service;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimulateDownStreamApplication.Controllers
{
    [ApiController]
    [Route("subject")]
    public class SubjectController : ControllerBase
    {
        private ILogger<StudentController> logger;

        private DemoService demoService;

        public SubjectController(ILogger<StudentController> logger, DemoService demoService)
        {
            this.logger = logger;
            this.demoService = demoService;
        }

        [HttpGet("duplicate")]
        public IActionResult GetStudentDuplicate()
        {
            //demoService.HelloWord();
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
            
            logger.LogInformation($"----- kiwi ----- Http method: {remoteIpAddress}.");
            var s = new List<Student>
            {
                new Student { Id=1, Name="subject", RollNumber = 100 },
                new Student { Id=2, Name="subjecteeee", RollNumber = 101 }
            };
            return Ok(s);
        }

        //[ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [HttpPut]
        [HttpPost]
        [HttpPatch]
        [HttpDelete]
        [Route("{resourceId}")]
        [Route("{resourceId}/{*url}")]
        public IActionResult ProxyCmkRequestToBackendAsync([FromRoute] string resourceId)
        {
            logger.LogInformation($"------------ {Request.Method}");
            throw new ArgumentException($"bad request value from user {resourceId}");
            return BadRequest();
            var s = new List<Student>
            {
                new Student { Id=1, Name="subject", RollNumber = 100 },
                new Student { Id=2, Name="subjecteeee", RollNumber = 101 }
            };
            return Ok(s);
        }
    }
}