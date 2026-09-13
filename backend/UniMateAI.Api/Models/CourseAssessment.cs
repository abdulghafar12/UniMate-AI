namespace UniMateAI.Api.Models;

public class CourseAssessment
{
    public int Id { get; set; }

    public int AcademicCourseId { get; set; }

    public string AssessmentType { get; set; } = string.Empty;

    public double MaxMarks { get; set; }

    public double ObtainedMarks { get; set; }

    public double Weightage { get; set; }

    public AcademicCourse? AcademicCourse { get; set; }
}