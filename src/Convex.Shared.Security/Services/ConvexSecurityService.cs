using Convex.Shared.Security.Interfaces;
using Convex.Shared.Security.Configuration;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Convex.Shared.Security.Services;

/// <summary>
/// Security service implementation for Convex microservices
/// </summary>
public class ConvexSecurityService : IConvexSecurityService
{
    private readonly ConvexSecurityOptions _options;
    private readonly JwtSecurityTokenHandler _tokenHandler;

    public ConvexSecurityService(IOptions<ConvexSecurityOptions> options)
    {
        _options = options.Value;
        _tokenHandler = new JwtSecurityTokenHandler();
    }

    public string GenerateJwtToken(string userId, string email, string[] roles, int expiresInMinutes = 60)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId),
            new(ClaimTypes.Email, email),
            new(ClaimTypes.Name, email),
            new("jti", Guid.NewGuid().ToString()),
            new("iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        // Add roles
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.JwtSecret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _options.JwtIssuer,
            audience: _options.JwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiresInMinutes),
            signingCredentials: credentials);

        return _tokenHandler.WriteToken(token);
    }

    public bool ValidateJwtToken(string token)
    {
        try
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.JwtSecret));
            var tokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _options.JwtIssuer,
                ValidAudience = _options.JwtAudience,
                IssuerSigningKey = key,
                ClockSkew = TimeSpan.Zero
            };

            _tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public ClaimsPrincipal? GetClaimsFromToken(string token)
    {
        try
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.JwtSecret));
            var tokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _options.JwtIssuer,
                ValidAudience = _options.JwtAudience,
                IssuerSigningKey = key,
                ClockSkew = TimeSpan.Zero
            };

            var principal = _tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
            return principal;
        }
        catch
        {
            return null;
        }
    }

    public string GenerateApiKey(string serviceName, DateTime? expiresAt = null)
    {
        var keyData = new
        {
            ServiceName = serviceName,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = expiresAt,
            Random = GenerateSecureRandomString(16)
        };

        var json = System.Text.Json.JsonSerializer.Serialize(keyData);
        var bytes = Encoding.UTF8.GetBytes(json);
        var base64 = Convert.ToBase64String(bytes);
        
        return $"convex_{base64}";
    }

    public bool ValidateApiKey(string apiKey)
    {
        try
        {
            if (!apiKey.StartsWith("convex_"))
                return false;

            var base64 = apiKey.Substring(7);
            var bytes = Convert.FromBase64String(base64);
            var json = Encoding.UTF8.GetString(bytes);
            var keyData = System.Text.Json.JsonSerializer.Deserialize<ApiKeyInfo>(json);

            if (keyData == null)
                return false;

            if (keyData.ExpiresAt.HasValue && keyData.ExpiresAt.Value < DateTime.UtcNow)
                return false;

            return keyData.IsActive;
        }
        catch
        {
            return false;
        }
    }

    public ApiKeyInfo? GetApiKeyInfo(string apiKey)
    {
        try
        {
            if (!apiKey.StartsWith("convex_"))
                return null;

            var base64 = apiKey.Substring(7);
            var bytes = Convert.FromBase64String(base64);
            var json = Encoding.UTF8.GetString(bytes);
            var keyData = System.Text.Json.JsonSerializer.Deserialize<ApiKeyInfo>(json);

            return keyData;
        }
        catch
        {
            return null;
        }
    }

    public string HashPassword(string password)
    {
        using var rng = RandomNumberGenerator.Create();
        var salt = new byte[32];
        rng.GetBytes(salt);

        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256);
        var hash = pbkdf2.GetBytes(32);

        var hashBytes = new byte[64];
        Array.Copy(salt, 0, hashBytes, 0, 32);
        Array.Copy(hash, 0, hashBytes, 32, 32);

        return Convert.ToBase64String(hashBytes);
    }

    public bool VerifyPassword(string password, string hash)
    {
        try
        {
            var hashBytes = Convert.FromBase64String(hash);
            var salt = new byte[32];
            Array.Copy(hashBytes, 0, salt, 0, 32);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256);
            var testHash = pbkdf2.GetBytes(32);

            for (int i = 0; i < 32; i++)
            {
                if (hashBytes[i + 32] != testHash[i])
                    return false;
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    public string GenerateSecureRandomString(int length = 32)
    {
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[length];
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }
}
