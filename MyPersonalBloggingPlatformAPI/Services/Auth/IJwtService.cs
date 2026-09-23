using PersonalBloggingPlatformAPI.Models;

namespace PersonalBloggingPlatformAPI.Services.Auth;

public interface IJwtService
{
    string GenerateToken(User user);
}