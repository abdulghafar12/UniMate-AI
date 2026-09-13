using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UniMateBackend.Data;
using UniMateAI.Api.DTOs;
using UniMateAI.Api.Models;

namespace UniMateAI.Api.Services;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);

    Task<AuthResponse> LoginAsync(LoginRequest request);
}

// Controllers should stay thin.
// This service contains the actual authentication logic.
public class AuthService : IAuthService
{
    private readonly UniMateDbContext _db;
    private readonly PasswordHasher<Student> _passwordHasher;

    public AuthService(UniMateDbContext db)
    {
        _db = db;
        _passwordHasher = new PasswordHasher<Student>();
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        // 1. Check whether passwords match.
        if (request.Password != request.ConfirmPassword)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "Passwords do not match."
            };
        }

        // 2. Check registration number.
        var regNumberExists = await _db.Students
            .AnyAsync(s =>
                s.RegistrationNumber == request.RegistrationNumber);

        if (regNumberExists)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "This registration number is already registered."
            };
        }

        // 3. Check university email.
        var emailExists = await _db.Students
            .AnyAsync(s =>
                s.UniversityEmail == request.UniversityEmail);

        if (emailExists)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "This university email is already registered."
            };
        }

        // 4. Create student object.
        var student = new Student
        {
            FullName = request.FullName,
            RegistrationNumber = request.RegistrationNumber,
            UniversityEmail = request.UniversityEmail,
            PersonalEmail = request.PersonalEmail,
            PhoneNumber = request.PhoneNumber,
            Program = request.Program,
            Department = request.Department,
            Semester = request.Semester,
            CreatedAt = DateTime.UtcNow
        };

        // 5. Hash the password.
        student.PasswordHash =
            _passwordHasher.HashPassword(student, request.Password);

        // 6. Save student.
        try
        {
            _db.Students.Add(student);
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "Unable to create account. Please try again."
            };
        }

        return new AuthResponse
        {
            Success = true,
            Message = "Student account created successfully."
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var identifier = request.Identifier.Trim();

        // Search by university email OR registration number.
        var student = await _db.Students
            .FirstOrDefaultAsync(s =>
                s.UniversityEmail == identifier ||
                s.RegistrationNumber == identifier);

        // Use one general message for security.
        if (student == null)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "Invalid email/registration number or password."
            };
        }

        // Verify the entered password against the saved hash.
        var passwordResult =
            _passwordHasher.VerifyHashedPassword(
                student,
                student.PasswordHash,
                request.Password
            );

        if (passwordResult == PasswordVerificationResult.Failed)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "Invalid email/registration number or password."
            };
        }
        return new AuthResponse
        {
            Success = true,
            Message = "Login successful!",

            Student = new StudentDto
            {
                Id = student.Id,
                FullName = student.FullName,
                RegistrationNumber = student.RegistrationNumber,
                UniversityEmail = student.UniversityEmail,
                PersonalEmail = student.PersonalEmail,
                PhoneNumber = student.PhoneNumber,
                Program = student.Program,
                Department = student.Department,
                Semester = student.Semester,
                CGPA = student.CGPA,
                CreatedAt = student.CreatedAt
            }
        };

    }
}