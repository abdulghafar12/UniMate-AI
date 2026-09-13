namespace UniMateAI.Api.DTOs;

public class CurriculumCourseDto
{
    public int Id { get; set; }
    public int SemesterNumber { get; set; }
    public string CourseId { get; set; } = string.Empty;
    public string CourseName { get; set; } = string.Empty;
    public string CreditHours { get; set; } = string.Empty;
    public bool IsLab { get; set; }
    public bool IsRequired { get; set; }
}