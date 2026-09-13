namespace UniMateAI.Api.DTOs;

public class AcademicCourseDto
{
    public int Id { get; set; }
    public string CourseId { get; set; } = string.Empty;
    public string CourseName { get; set; } = string.Empty;
    public string CreditHours { get; set; } = string.Empty;
    public bool IsLab { get; set; }
    public bool IsRetake { get; set; }
    public string? PreviousGrade { get; set; }
    public string Grade { get; set; } = string.Empty;
    public string ResultStatus { get; set; } = string.Empty;
}