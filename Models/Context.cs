using Microsoft.EntityFrameworkCore;
using Models;

public class Context : DbContext
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<Mark> Marks { get; set; }

    public Context(DbContextOptions<Context> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
         modelBuilder.Entity<Enrollment>()
        .HasKey(e => e.Id);

    modelBuilder.Entity<Enrollment>()
        .HasOne(e => e.Student)
        .WithMany()
        .HasForeignKey(e => e.StudentId);

    modelBuilder.Entity<Enrollment>()
        .HasOne(e => e.Course)
        .WithMany()
        .HasForeignKey(e => e.CourseId);
    }
    
}