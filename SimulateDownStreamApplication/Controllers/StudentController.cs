using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimulateDownStreamApplication.Model;
using SimulateDownStreamApplication.Repository;
using System;
using System.Collections.Generic;

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
        public IActionResult AddStudent(Student student)
        {
            try
            {
                Student response = _repository.AddStudent(student);
                return CreatedAtRoute("GetStudent", new
                {
                    id = response.Id
                }, response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"exception in AddStudent, {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("1")]
        public IActionResult GetStudents()
        {
            return Ok(_repository.GetStudents());
        }

        [HttpGet("2")]
        public IActionResult GetStudentDuplicate()
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
        public IActionResult GetStudent(int id)
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
        public IActionResult DeleteStudent(int id)
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
        public IActionResult UpdateStudent(int id, Student student)
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
        public IActionResult PatchStudent(int id, Student student)
        {
            return UpdateStudent(id, student);
        }

        [HttpGet("mock")]
        public IEnumerable<string> Get()
        {
            _logger.LogInformation("********************* Get call revceived");
            throw new ArgumentException();

            //return new List<string>() { "GET", "apple", "banana" };
        }

        [HttpPost("mock")]
        public IEnumerable<string> Post()
        {
            _logger.LogInformation("^^^^^^^^^^^^^^^^^^^^^^^^^ Post call revceived");
            throw new ArgumentException();

            //return new List<string>() { "POST", "apple", "banana" };
        }
    }
}