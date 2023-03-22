using System.ComponentModel.DataAnnotations;

namespace Models;
public class Enrollment
{
    [Key]
    public int Id { get; set; }
    public int StudentId { get; set; }
    public Student? Student { get; set; }
    public int CourseId { get; set; }
    public Course? Course { get; set; }
    [Range(6, 10, ErrorMessage = "Mark must be between 6 and 10.")]
    public int? Mark { get; set; }

}
