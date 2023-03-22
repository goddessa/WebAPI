using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Students.Controllers;

[ApiController]
[Route("[controller]")]
public class EnrollmentController : ControllerBase
{
    
    public Context Context { get; set; }
    public EnrollmentController(Context context)
    {
        Context = context;
    }
    //CRUD for Enrollment
    //CREATE  
    [HttpPost("AddEnrollment")]
    public async Task<IActionResult> AddEnrollment([FromBody] Enrollment enrollment)
    {
        try
        {
            var existingEnrollment = await Context.Enrollments
                .Where(e => e.StudentId == enrollment.StudentId && e.CourseId == enrollment.CourseId)
                .FirstOrDefaultAsync();

            if (existingEnrollment != null)
            {
                return BadRequest("Enrollment already exists.");
            }

            if (enrollment.Mark.HasValue && (enrollment.Mark.Value < 6 || enrollment.Mark.Value > 10))
            {
                return BadRequest("Mark must be between 6 and 10.");
            }

            await Context.Enrollments.AddAsync(enrollment);
            await Context.SaveChangesAsync();

            return Ok($"Enrollment added for student {enrollment.StudentId} in course {enrollment.CourseId}.");
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    //Get enrollment ako prosledimo id kursa ili id studenta, bilo koje od ta 2
    [HttpGet("GetEnrollmentsByIds")]
    public async Task<ActionResult<IEnumerable<Enrollment>>> GetEnrollmentsByIds(int? courseId = null, int? studentId = null)
    {
        var enrollmentsQuery = Context.Enrollments.AsQueryable();

        if (courseId != null)
        {
            enrollmentsQuery = enrollmentsQuery.Where(e => e.CourseId == courseId);
        }

        if (studentId != null)
        {
            enrollmentsQuery = enrollmentsQuery.Where(e => e.StudentId == studentId);
        }

        var enrollments = await enrollmentsQuery
            .Include(e => e.Course)
            .Include(e => e.Student)
            .ToListAsync();

        var jsonOptions = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve
        };

        var serializedEnrollments = JsonSerializer.Serialize(enrollments, jsonOptions);

        return Ok(serializedEnrollments);
    }

    //Get sve enrollments
    [HttpGet]
    [Produces("application/json")]
    public IActionResult GetEnrollments()
    {
        var enrollments = Context.Enrollments
            .Include(e => e.Student)
            .Include(e => e.Course)
            .ToList();
        
        return Ok(enrollments);
    }

    //Get kurseve za 1 studenta
    [HttpGet("Student/{studentId}/Courses")]
    public IActionResult GetStudentCourses(int studentId)
    {
        var studentCourses = Context.Enrollments
            .Include(e => e.Course)
            .Where(e => e.StudentId == studentId)
            .Select(e => e.Course)
            .ToList();
        
        return Ok(studentCourses);
    }

    //Get sve studente na kursu
    [HttpGet("Course/{courseId}/Students")]
    public IActionResult GetCourseStudents(int courseId)
    {
        var courseStudents = Context.Enrollments
            .Include(e => e.Student)
            .Where(e => e.CourseId == courseId)
            .Select(e => e.Student)
            .ToList();
        
        return Ok(courseStudents);
    }


    //UPDATE
    [HttpPut("UpdateEnrollment/{id}")]
    public async Task<IActionResult> UpdateEnrollment(int id, [FromBody] Enrollment enrollment)
    {
        try
        {
            if (id != enrollment.Id)
            {
                return BadRequest("Enrollment ID does not match.");
            }

            var existingEnrollment = await Context.Enrollments.FindAsync(id);

            if (existingEnrollment == null)
            {
                return NotFound($"Enrollment with ID {id} not found.");
            }

            existingEnrollment.StudentId = enrollment.StudentId;
            existingEnrollment.CourseId = enrollment.CourseId;
            existingEnrollment.Mark = enrollment.Mark;

            await Context.SaveChangesAsync();

            return Ok($"Enrollment with ID {id} updated.");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }


    //DELETE
    [HttpDelete("DeleteEnrollment/{enrollmentId}")]
    public async Task<IActionResult> DeleteEnrollment(int enrollmentId)
    {
        try
        {
            var enrollment = await Context.Enrollments.FindAsync(enrollmentId);

            if (enrollment == null)
            {
                return NotFound($"Enrollment with ID {enrollmentId} not found.");
            }

            Context.Enrollments.Remove(enrollment);
            await Context.SaveChangesAsync();

            return Ok($"Enrollment with ID {enrollmentId} deleted.");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }




}