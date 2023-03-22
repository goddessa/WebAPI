using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models;

public class Student
{
    [Key]
    public int ID { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }	
    public required string Address { get; set; }
    public required string City { get; set; }
    public required string Country { get; set; }
    public DateTime DateOfBirth { get; set; }
    [JsonIgnore]
    public ICollection<Enrollment>? Enrollments { get; set; }
}