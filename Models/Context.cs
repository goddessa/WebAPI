using Microsoft.EntityFrameworkCore;
using Models;

public class Context : DbContext
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }

    public Context(DbContextOptions<Context> options) : base(options)
    {
    }
    public void ConfigureServices(IServiceCollection services)
        => services.AddDbContext<Context>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne<Student>(e => e.Student).WithMany(student => student.Enrollments).HasForeignKey(e => e.StudentId);
            entity.HasOne<Course>(e => e.Course).WithMany(course => course.Enrollments).HasForeignKey(e => e.CourseId);

        });
        
    }
    
}