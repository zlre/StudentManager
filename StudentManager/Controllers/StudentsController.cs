namespace StudentManager.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using StudentManager.Repositories;
    using StudentManagerData;

    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private IStudentsRepository _repository;

        public StudentsController(IStudentsRepository students)
        {
            _repository = students;
        }

        private Guid UserId {
            get {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                return Guid.Parse(claimsIdentity.FindFirst(ClaimTypes.Name)?.Value);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> Get()
        {
            var students  = await _repository.GetAllUserStudentsAsync(UserId);

            if (students != null)
            {
                return Ok(students);
            }

            return BadRequest(new { message = $"Студенты не найдены" });

        }        
        
        [HttpGet("count")]
        public async Task<ActionResult<int>> Count()
        {
            var count  = await _repository.GetAllStudentsCount(UserId);

            return Ok(count);
        }

        [HttpGet("{id}", Name = "Get")]
        public async Task<ActionResult<Student>> Get(Guid id)
        {
            var student = await _repository.GetStudentAsync(id);
            if (student != null)
            {
                return Ok(student);
            }

            return BadRequest(new { message = $"Студент не существует" });
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Student student)
        {
            var newStudent = await _repository.AddStudentAsync(UserId, student);

            if (newStudent == null)
            {
                return BadRequest(new { message = $"Студент с идентификатором {student.StudentID} уже существует" });
            }
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] Student student)
        {
            await _repository.UpdateStudentAsync(id, student);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _repository.DeleteStudentAsync(id);
            return Ok();
        }
    }
}
