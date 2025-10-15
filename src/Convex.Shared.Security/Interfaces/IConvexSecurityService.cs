using System.Security.Claims;

namespace Convex.Shared.Security.Interfaces;

/// <summary>
/// Security service interface for Convex microservices
/// </summary>
public interface IConvexSecurityService
{
    /// <summary>
    /// Generates a JWT token for the specified user
    /// </summary>
    /// <param name="userId">The user ID</param>
    /// <param name="email">The user email</param>
    /// <param name="roles">The user roles</param>
    /// <param name="expiresInMinutes">Token expiration in minutes</param>
    /// <returns>JWT token</returns>
    string GenerateJwtToken(string userId, string email, string[] roles, int expiresInMinutes = 60);

    /// <summary>
    /// Validates a JWT token
    /// </summary>
    /// <param name="token">The JWT token to validate</param>
    /// <returns>True if valid</returns>
    bool ValidateJwtToken(string token);

    /// <summary>
    /// Gets claims from a JWT token
    /// </summary>
    /// <param name="token">The JWT token</param>
    /// <returns>Claims principal</returns>
    ClaimsPrincipal? GetClaimsFromToken(string token);

    /// <summary>
    /// Generates an API key
    /// </summary>
    /// <param name="serviceName">The service name</param>
    /// <param name="expiresAt">Optional expiration date</param>
    /// <returns>API key</returns>
    string GenerateApiKey(string serviceName, DateTime? expiresAt = null);

    /// <summary>
    /// Validates an API key
    /// </summary>
    /// <param name="apiKey">The API key to validate</param>
    /// <returns>True if valid</returns>
    bool ValidateApiKey(string apiKey);

    /// <summary>
    /// Gets service information from API key
    /// </summary>
    /// <param name="apiKey">The API key</param>
    /// <returns>Service information</returns>
    ApiKeyInfo? GetApiKeyInfo(string apiKey);

    /// <summary>
    /// Hashes a password
    /// </summary>
    /// <param name="password">The password to hash</param>
    /// <returns>Hashed password</returns>
    string HashPassword(string password);

    /// <summary>
    /// Verifies a password against its hash
    /// </summary>
    /// <param name="password">The password to verify</param>
    /// <param name="hash">The hash to verify against</param>
    /// <returns>True if password matches</returns>
    bool VerifyPassword(string password, string hash);

    /// <summary>
    /// Generates a secure random string
    /// </summary>
    /// <param name="length">The length of the string</param>
    /// <returns>Secure random string</returns>
    string GenerateSecureRandomString(int length = 32);
}

/// <summary>
/// API key information
/// </summary>
public class ApiKeyInfo
{
    /// <summary>
    /// The service name
    /// </summary>
    public string ServiceName { get; set; } = string.Empty;

    /// <summary>
    /// The creation date
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// The expiration date
    /// </summary>
    public DateTime? ExpiresAt { get; set; }

    /// <summary>
    /// Whether the key is active
    /// </summary>
    public bool IsActive { get; set; } = true;
}
