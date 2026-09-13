using Microsoft.EntityFrameworkCore;
using UniMateAI.Api.Models;
using UniMateBackend.Models;

namespace UniMateBackend.Data;

public class UniMateDbContext : DbContext
{
    public UniMateDbContext(DbContextOptions<UniMateDbContext> options)
        : base(options)
    {
    }

    public DbSet<Conversation> Conversations => Set<Conversation>();

    public DbSet<ChatMessage> Messages => Set<ChatMessage>();

    public DbSet<Student> Students => Set<Student>();

    public DbSet<AcademicSemester> AcademicSemesters => Set<AcademicSemester>();

    public DbSet<AcademicCourse> AcademicCourses => Set<AcademicCourse>();

    public DbSet<CurriculumCourse> CurriculumCourses => Set<CurriculumCourse>();

    public DbSet<CourseAttendance> CourseAttendanceRecords => Set<CourseAttendance>();

    public DbSet<CourseAssessment> CourseAssessments => Set<CourseAssessment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Conversation>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.Property(c => c.StudentId)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(c => c.Tool)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(c => c.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.HasIndex(c => new
            {
                c.StudentId,
                c.Tool
            });
        });

        modelBuilder.Entity<ChatMessage>(entity =>
        {
            entity.HasKey(m => m.Id);

            entity.Property(m => m.Role)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(m => m.Content)
                .IsRequired();

            entity.HasOne(m => m.Conversation)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}