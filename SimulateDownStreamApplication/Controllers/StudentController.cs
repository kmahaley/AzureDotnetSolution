using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimulateDownStreamApplication.Model;
using SimulateDownStreamApplication.Repository;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SimulateDownStreamApplication.Controllers
{
    [ApiController]
    [Route("student")]
    public class StudentController : ControllerBase
    {
        private ILogger<StudentController> _logger;

        private IStudentRepository _repository;

        public StudentController(ILogger<StudentController> logger, IStudentRepository repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpPost]
        public IActionResult AddStudent(Student student, CancellationToken token = default)
        {
            try
            {
                Student response = _repository.AddStudent(student);
                return CreatedAtRoute("GetStudent", new
                {
                    id = response.Id
                }, response);
            }
            catch(Exception ex)
            {
                _logger.LogError($"exception in AddStudent, {ex.Message}");
                throw;
            }
        }

        [HttpGet("1")]
        public async Task<IActionResult> GetStudents(CancellationToken token = default)
        {
            _logger.LogError(" Request received -------------------");
            try
            {
                
                await Task.Delay(10000, token);
                return Ok(_repository.GetStudents());
            }
            catch(Exception ex)
            {
                _logger.LogError($"exception in Getting student,{ex.GetType()} : {ex.Message}");
                throw;
            }
            
        }

        [HttpGet("2")]
        public IActionResult GetStudentDuplicate(CancellationToken token = default)
        {
            var s = new List<Student>
            {
                new Student { Id=1, Name="duplicate", RollNumber = 100 },
                new Student { Id=2, Name="duplicateeeee", RollNumber = 101 }
            };
            //Thread.Sleep(1200);
            return Ok(s);
        }

        [HttpGet("{id}", Name = "GetStudent")]
        public IActionResult GetStudent(int id, CancellationToken token = default)
        {
            try
            {
                Student response = _repository.GetStudent(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"exception in GetStudent, {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id, CancellationToken token = default)
        {
            bool isDeleted = _repository.DeleteStudent(id);
            if (isDeleted)
            {
                return NoContent();
            }
            _logger.LogError($"Student not present: DeleteStudent, {id}");
            return BadRequest(id);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateStudent(int id, Student student, CancellationToken token = default)
        {
            try
            {
                Student response = _repository.UpdateStudent(id, student);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"exception in UpdateStudent, {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public IActionResult PatchStudent(int id, Student student, CancellationToken token = default)
        {
            return UpdateStudent(id, student);
        }

        [HttpGet("mock")]
        public IEnumerable<string> Get(CancellationToken token = default)
        {
            _logger.LogInformation("********************* Get call revceived");
            //throw new ArgumentException();

            return new List<string>() { "GET", "apple", "banana" };
        }

        [HttpPost("mock")]
        public IEnumerable<string> Post(CancellationToken token = default)
        {
            _logger.LogInformation("^^^^^^^^^^^^^^^^^^^^^^^^^ Post call revceived");
            throw new ArgumentException();

            //return new List<string>() { "POST", "apple", "banana" };
        }
    }
}