namespace UniMateAI.Api.Models;

// This class represents ONE ROW in the "Students" table.
// Entity Framework Core reads this class and creates the matching SQL table for us.
public class Student
{
    // Primary key. EF Core auto-increments this because it's called "Id" and is an int.
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    // Must be unique — enforced both in code (AuthService) and in the database (see UniMateDbContext).
    public string RegistrationNumber { get; set; } = string.Empty;

    // Must be unique — same as above.
    public string UniversityEmail { get; set; } = string.Empty;

    // Optional field, so it's nullable.
    public string? PersonalEmail { get; set; }

    public string? PhoneNumber { get; set; }

    // We NEVER store the raw password. Only this hashed version is saved.
    public string PasswordHash { get; set; } = string.Empty;

    public string Program { get; set; } = string.Empty;

    public string Department { get; set; } = string.Empty;

    public int Semester { get; set; }

    // Starts at 0.0 — will be calculated later once grades exist.
    public double CGPA { get; set; } = 0.0;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
