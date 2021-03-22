using Microsoft.AspNetCore.Mvc;
using SimulateDownStreamApplication.Model;
using System.Collections.Generic;

namespace SimulateDownStreamApplication.Controllers
{
    [ApiController]
    [Route("subject")]
    public class SubjectController : ControllerBase
    {
        [HttpGet("2")]
        public IActionResult GetStudentDuplicate()
        {
            var s = new List<Student>
            {
                new Student { Id=1, Name="subject", RollNumber = 100 },
                new Student { Id=2, Name="subjecteeee", RollNumber = 101 }
            };
            return Ok(s);
        }
    }
}