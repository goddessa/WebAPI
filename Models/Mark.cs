using System.ComponentModel.DataAnnotations;

namespace Models;

public class Mark
{
    [Key]
    public int ID { get; set; }
    public int Value { get; set; }
    public Enrollment? Enrollment { get; set; }

}