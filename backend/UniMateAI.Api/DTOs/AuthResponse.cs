namespace UniMateAI.Api.DTOs;

public class AuthResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public StudentDto? Student { get; set; }
}