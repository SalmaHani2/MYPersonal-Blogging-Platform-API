using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using PersonalBloggingPlatformAPI.Data;
using PersonalBloggingPlatformAPI.DTOs.Auth;
using PersonalBloggingPlatformAPI.Models;

namespace PersonalBloggingPlatformAPI.Services.Auth;

public class AuthService : IAuthService
{
	private readonly ApplicationDbContext _context;
	private readonly IJwtService _jwtService;
	private readonly ILogger<AuthService> _logger;

	public AuthService(
		ApplicationDbContext context,
		IJwtService jwtService,
		ILogger<AuthService> logger)
	{
		_context = context;
		_jwtService = jwtService;
		_logger = logger;
	}

	public async Task<User?> RegisterAsync(RegisterDto dto)
	{
		// check duplicate email
		if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
		{
			return null;
		}

		var (hash, salt) = CreatePasswordHash(dto.Password);

		var user = new User
		{
			Username = dto.Username,
			Email = dto.Email,
			PasswordHash = Convert.ToBase64String(hash),
			CreatedAt = DateTime.UtcNow
		};

		// store salt appended to hash in a separate column would be ideal; keeping simple: prefix salt to hash
		// We'll store combined as salt:hash
		user.PasswordHash = Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);

		_context.Users.Add(user);
		await _context.SaveChangesAsync();

		_logger.LogInformation("Registered new user {Email}", user.Email);

		return user;
	}

	public async Task<string?> LoginAsync(LoginDto dto)
	{
		var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
		if (user == null)
		{
			return null;
		}

		if (!VerifyPassword(dto.Password, user.PasswordHash))
		{
			return null;
		}

		// generate token
		var token = _jwtService.GenerateToken(user);

		return token;
	}

	private static (byte[] hash, byte[] salt) CreatePasswordHash(string password)
	{
		using var rng = RandomNumberGenerator.Create();
		var salt = new byte[16];
		rng.GetBytes(salt);

		using var deriveBytes = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
		var hash = deriveBytes.GetBytes(32);

		return (hash, salt);
	}

	private static bool VerifyPassword(string password, string stored)
	{
		try
		{
			var parts = stored.Split(':');
			if (parts.Length != 2) return false;

			var salt = Convert.FromBase64String(parts[0]);
			var hash = Convert.FromBase64String(parts[1]);

			using var deriveBytes = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
			var testHash = deriveBytes.GetBytes(32);

			return CryptographicOperations.FixedTimeEquals(testHash, hash);
		}
		catch
		{
			return false;
		}
	}
}

