using System.ComponentModel.DataAnnotations;

namespace UniMateAI.Api.DTOs;

public class LoginRequest
{
    [Required(ErrorMessage = "Email or registration number is required.")]
    public string Identifier { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; } = string.Empty;
}