namespace UniMateAI.Api.Models;

public class CourseAttendance
{
    public int Id { get; set; }

    public int AcademicCourseId { get; set; }

    public DateTime AttendanceDate { get; set; }

    public string Status { get; set; } = "Present";

    public AcademicCourse? AcademicCourse { get; set; }
}