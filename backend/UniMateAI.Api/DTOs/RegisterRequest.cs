using System.ComponentModel.DataAnnotations;

namespace UniMateAI.Api.DTOs;

// DTO = Data Transfer Object.
// This is the SHAPE of the JSON the frontend is allowed to send us.
// It is NOT the same class as Student — that's intentional (see explanation below).
// The [Required]/[EmailAddress] attributes give us free server-side validation.
public class RegisterRequest
{
    [Required(ErrorMessage = "Full name is required.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Registration number is required.")]
    public string RegistrationNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "University email is required.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    public string UniversityEmail { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Enter a valid personal email address.")]
    public string? PersonalEmail { get; set; }

    public string? PhoneNumber { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please confirm your password.")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Program is required.")]
    public string Program { get; set; } = string.Empty;

    [Required(ErrorMessage = "Department is required.")]
    public string Department { get; set; } = string.Empty;

    [Required(ErrorMessage = "Semester is required.")]
    [Range(1, 8, ErrorMessage = "Semester must be between 1 and 8.")]
    public int Semester { get; set; }
}
