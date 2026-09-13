namespace UniMateAI.Api.DTOs;

public class StudentDto
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string RegistrationNumber { get; set; } = string.Empty;

    public string UniversityEmail { get; set; } = string.Empty;

    public string? PersonalEmail { get; set; }

    public string? PhoneNumber { get; set; }

    public string Program { get; set; } = string.Empty;

    public string Department { get; set; } = string.Empty;

    public int Semester { get; set; }

    public double CGPA { get; set; }

    public DateTime CreatedAt { get; set; }
}