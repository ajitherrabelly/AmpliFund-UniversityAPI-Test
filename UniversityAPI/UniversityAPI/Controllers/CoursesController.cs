using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityAPI.Data;
using UniversityAPI.Helpers;
using UniversityAPI.Models;

namespace UniversityAPI.Controllers
{
    [ApiController]
    [Route("University/[controller]")]
    public class CoursesController : ControllerBase
    {

        private readonly UniversityContext _context;

        public CoursesController(UniversityContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses([FromQuery] PaginationParameters parameters)
        {
            return await _context.Courses
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();
        }

        // GET: api/courses/{externalCourseId}
        [HttpGet("{externalCourseId}")]
        public async Task<ActionResult<Course>> GetCourse(Guid externalCourseId)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.ExternalCourseId == externalCourseId);

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }

        // POST: api/courses
        [HttpPost]
        public async Task<ActionResult<Course>> CreateCourse(Course course)
        {
            course.ExternalCourseId = Guid.NewGuid();
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCourse), new { externalCourseId = course.ExternalCourseId }, course);
        }

        [HttpPut("{externalCourseId}")]
        public async Task<IActionResult> UpdateCourse(Guid externalCourseId, Course course)
        {
            if (externalCourseId != course.ExternalCourseId)
            {
                return BadRequest();
            }

            _context.Entry(course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(externalCourseId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{externalCourseId}")]
        public async Task<IActionResult> DeleteCourse(Guid externalCourseId)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.ExternalCourseId == externalCourseId);
            if (course == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/courses/{externalCourseId}/enroll/{externalStudentId}
        [HttpPost("{externalCourseId}/enroll/{externalStudentId}")]
        public async Task<IActionResult> EnrollStudent(Guid externalCourselId, Guid externalStudentId)
        {
            var course = await _context.Courses.Include(c => c.EnrolledStudents)
                .FirstOrDefaultAsync(c => c.ExternalCourseId == externalCourselId);
            var student = await _context.Students.FirstOrDefaultAsync(s => s.ExternalStudentlId == externalStudentId);

            if (course == null || student == null)
            {
                return NotFound();
            }

            if (course.CurrentEnrollment >= course.MaxCapacity)
            {
                return BadRequest("Course is at maximum capacity");
            }

            course.EnrolledStudents.Add(student);
            course.CurrentEnrollment++;
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool CourseExists(Guid externalCourseId)
        {
            return _context.Courses.Any(e => e.ExternalCourseId == externalCourseId);
        }
    }
}
