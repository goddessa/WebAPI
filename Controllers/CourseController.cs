using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Students.Controllers;

[ApiController]
[Route("[controller]")]
public class CourseController : ControllerBase
{
    public Context Context { get; set; }
    public CourseController(Context context)
    {
        Context = context;
    }

    //CRUD for Courses
    //CREATE
    [HttpPost("AddCourse")]
    public async Task<ActionResult> AddCourse([FromBody]Course course)
    {
        try
        {
            var existingCourse = await Context.Courses.FirstOrDefaultAsync(c => c.Code == course.Code);

            if (existingCourse != null)
            {
                return BadRequest($"Course with code '{course.Code}' already exists.");
            }

            await Context.Courses.AddAsync(course);
            await Context.SaveChangesAsync();

            return Ok($"ID of new course is: {course.ID}");
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }


    //READ
    [HttpGet("GetCourses")]
    public async Task<ActionResult> GetCourses()
    {
         try
        {
            return Ok(await Context
            .Courses
            .ToListAsync());
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    //UPDATE
    [HttpPut("UpdateCourse/{courseId}")]
    public async Task<ActionResult> UpdateCourse([FromBody]Course course, int courseId )
    {
        try
        {
            var oldCourse = await Context.Courses.FindAsync(courseId);
            if(oldCourse != null)
            {
                oldCourse.Code = course.Code;
                oldCourse.Name = course.Name;
                oldCourse.Description = course.Description;
                Context.Courses.Update(oldCourse);
                await Context.SaveChangesAsync();
                return Ok($"Id of changed course is : {courseId}");

            }
            else
            {
                return BadRequest("Error! ");
            }
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    //DELETE
    [HttpDelete("DeleteCourse/{idCourse}")]
    public async Task<ActionResult> DeleteCourse(int idCourse)
    {
        try
        {
            var p = await Context.Courses.FindAsync(idCourse);
            if(p != null)
            {
                Context.Courses.Remove(p);
                await Context.SaveChangesAsync();
                return Ok($"Id of deleted course is : {idCourse}");
            }
            else
            {
                return BadRequest("Error");
            }
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

   

}