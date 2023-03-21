using System.ComponentModel.DataAnnotations;

namespace Models;

public class Course
{
    [Key]
    public int ID { get; set; }
    public required string Code { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public List<Enrollment>? Enrollments { get; set; }
}