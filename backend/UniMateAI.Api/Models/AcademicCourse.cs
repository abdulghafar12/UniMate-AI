namespace UniMateAI.Api.Models;

public class AcademicCourse
{
    public int Id { get; set; }

    public int AcademicSemesterId { get; set; }

    public string CourseId { get; set; } = string.Empty;

    public string CourseName { get; set; } = string.Empty;

    public string CreditHours { get; set; } = string.Empty;

    public bool IsLab { get; set; }

    public bool IsRetake { get; set; }

    public string? PreviousGrade { get; set; }

    public string Grade { get; set; } = "Result Awaited";

    public double? GradePoint { get; set; }

    public string ResultStatus { get; set; } = "Result Awaited";

    public AcademicSemester? AcademicSemester { get; set; }

    public List<CourseAssessment> Assessments { get; set; } = new();

    public List<CourseAttendance> AttendanceRecords { get; set; } = new();
}