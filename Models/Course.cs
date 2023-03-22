using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models;

public class Course
{
    [Key]
    public int ID { get; set; }
    public required string Code { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    [JsonIgnore]
    public ICollection<Enrollment>? Enrollments { get; set; }
}