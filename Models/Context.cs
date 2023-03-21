using Microsoft.EntityFrameworkCore;
using Models;

public class Context : DbContext
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Mark> Marks { get; set; }

    public Context(DbContextOptions<Context> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Mark>()
            .HasKey(e => new { e.StudentId, e.CourseId }); //praćenje ocena studenata na datom kursu, many - to - many
    }
}