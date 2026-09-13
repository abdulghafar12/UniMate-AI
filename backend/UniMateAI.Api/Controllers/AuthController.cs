using Microsoft.AspNetCore.Mvc;
using UniMateAI.Api.DTOs;
using UniMateAI.Api.Services;

namespace UniMateAI.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    // POST /api/auth/register
    // This is the endpoint your register.html JavaScript will call with fetch().
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        // [ApiController] + the [Required]/[EmailAddress] attributes on RegisterRequest
        // mean invalid JSON (missing fields, bad email format) is automatically rejected
        // with a 400 Bad Request BEFORE this method body even runs.
        // ModelState.IsValid lets us double check and return our own clean message shape.
        if (!ModelState.IsValid)
        {
            var firstError = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .FirstOrDefault() ?? "Please complete all required fields.";

            return BadRequest(new AuthResponse { Success = false, Message = firstError });
        }

        var result = await _authService.RegisterAsync(request);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    // STEP 3 will add:
    // [HttpPost("login")]
    // public async Task<IActionResult> Login([FromBody] LoginRequest request) { ... }
[HttpPost("login")]
public async Task<IActionResult> Login([FromBody] LoginRequest request)
{
    if (!ModelState.IsValid)
    {
        var firstError = ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .FirstOrDefault() ?? "Please enter your login information.";

        return BadRequest(new AuthResponse
        {
            Success = false,
            Message = firstError
        });
    }

    var result = await _authService.LoginAsync(request);

    if (!result.Success)
    {
        return Unauthorized(result);
    }

    return Ok(result);
}
}
