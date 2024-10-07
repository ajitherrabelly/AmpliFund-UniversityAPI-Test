using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityAPI.Data;
using UniversityAPI.Helpers;
using UniversityAPI.Models;

namespace UniversityAPI.Controllers
{
    [Route("University/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {

        private readonly UniversityContext _context;
        private readonly ILogger<StudentController> _logger;

        public StudentController(UniversityContext context, ILogger<StudentController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents(
            [FromQuery] PaginationParameters parameters,
            [FromQuery] string sortBy = "FirstName",
            [FromQuery] string filter = "")
        {
            _logger.LogInformation("Getting students with parameters: {@Parameters}", new { parameters, sortBy, filter });

            var students = _context.Students.AsQueryable();

            if (!string.IsNullOrEmpty(filter))
            {
                students = students.Where(s => s.FirstName.Contains(filter) || s.LastName.Contains(filter) || s.Email.Contains(filter));
            }

            students = sortBy.ToLower() switch
            {
                "firstname" => students.OrderBy(s => s.FirstName),
                "lastname" => students.OrderBy(s => s.LastName),
                "email" => students.OrderBy(s => s.Email),
                "dateofbirth" => students.OrderBy(s => s.DateOfBirth),
                _ => students.OrderBy(s => s.ExternalId)
            };

            return await students
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();
        }

        [HttpGet("{externalId}")]
        public async Task<ActionResult<Student>> GetStudent(Guid externalId)
        {
            var student = await _context.Students.FindAsync(externalId);

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }

        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            student.ExternalId = Guid.NewGuid();
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStudent), new { externalId = student.ExternalId }, student);
        }

    }
}
