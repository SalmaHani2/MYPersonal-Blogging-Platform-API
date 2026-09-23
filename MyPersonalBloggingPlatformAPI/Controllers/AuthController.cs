using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PersonalBloggingPlatformAPI.DTOs.Auth;
using PersonalBloggingPlatformAPI.Services.Auth;
using PersonalBloggingPlatformAPI.Models;

namespace PersonalBloggingPlatformAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var user = await _authService.RegisterAsync(dto);
        if (user == null)
        {
            return Conflict(new { message = "Email already in use." });
        }

        return CreatedAtAction(nameof(Register), new { id = user.Id }, new { id = user.Id, username = user.Username, email = user.Email });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var token = await _authService.LoginAsync(dto);
        if (token == null)
        {
            return Unauthorized(new { message = "Invalid credentials." });
        }

        return Ok(new { token });
    }
}
