using PersonalBloggingPlatformAPI.DTOs.Auth;
using PersonalBloggingPlatformAPI.Models;

namespace PersonalBloggingPlatformAPI.Services.Auth;

public interface IAuthService
{
    Task<User?> RegisterAsync(RegisterDto dto);

    Task<string?> LoginAsync(LoginDto dto);
}
