using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Students.Controllers;

[ApiController]
[Route("[controller]")]
public class StudentController : ControllerBase
{
    
    public Context Context { get; set; }
    public StudentController(Context context)
    {
        Context = context;
    }

    //CRUD operations for Student
    //CREATE
    [HttpPost("AddStudent")]
    public async Task<ActionResult> AddStudent([FromBody]Student student)
    {
        try
        {
            var existingStudent = await Context.Students
            .FirstOrDefaultAsync(e => e.ID == student.ID);
            if(existingStudent != null)
            {
                return BadRequest($"Student with ID '{student.ID}' already exists!");
            }
            else
            {   
                await Context.Students.AddAsync(student);
                await Context.SaveChangesAsync();
                return Ok($"ID of new student is: {student.ID}");
            }
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }

    }

    //READ
    [HttpGet("GetStudents")]
    public async Task<ActionResult> GetStudents()
    {
        try
        {
            return Ok(await Context
            .Students
            .ToListAsync());
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    //UPDATE
    [HttpPut("UpdateStudent/{studentID}")]
    public async Task<ActionResult> UpdateStudent([FromBody]Student student, int studentID)
    {
        try
        {
            var oldStudent = await Context.Students.FindAsync(studentID);
            if(oldStudent != null)
            {
                oldStudent.FirstName = student.FirstName;
                oldStudent.LastName = student.LastName;
                oldStudent.Address = student.Address;
                oldStudent.City = student.City;
                oldStudent.Country = student.Country;
                oldStudent.DateOfBirth = student.DateOfBirth;
                Context.Students.Update(oldStudent);
                await Context.SaveChangesAsync();
                return Ok($"ID of updated student is: {studentID}");
            }
            else
            {
                return BadRequest("Error!");
            }
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    //DELETE
    [HttpDelete("DeleteStudent/{IdStudent}")]
    public async Task<ActionResult> DeleteStudent(int IdStudent)
    {
        try
        {
            var emp = await Context.Students.FindAsync(IdStudent);
            if(emp != null)
            {
                Context.Students.Remove(emp);
                await Context.SaveChangesAsync();
                return Ok($"ID of deleted employee is: {IdStudent}");
            }
            else
            {
                return BadRequest($"Not found employee with ID : {IdStudent}");
            }
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }

}
