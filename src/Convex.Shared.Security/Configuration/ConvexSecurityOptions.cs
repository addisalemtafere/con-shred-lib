namespace Convex.Shared.Security.Configuration;

/// <summary>
/// Configuration options for Convex security services
/// </summary>
public class ConvexSecurityOptions
{
    /// <summary>
    /// JWT secret key for token signing
    /// </summary>
    public string JwtSecret { get; set; } = string.Empty;

    /// <summary>
    /// JWT issuer
    /// </summary>
    public string JwtIssuer { get; set; } = "Convex";

    /// <summary>
    /// JWT audience
    /// </summary>
    public string JwtAudience { get; set; } = "ConvexUsers";

    /// <summary>
    /// JWT expiration time in minutes
    /// </summary>
    public int JwtExpirationMinutes { get; set; } = 60;

    /// <summary>
    /// API key secret for service authentication
    /// </summary>
    public string ApiKeySecret { get; set; } = string.Empty;

    /// <summary>
    /// Password hashing iterations
    /// </summary>
    public int PasswordHashIterations { get; set; } = 100000;

    /// <summary>
    /// Whether to require HTTPS
    /// </summary>
    public bool RequireHttps { get; set; } = true;

    /// <summary>
    /// CORS allowed origins
    /// </summary>
    public string[] AllowedOrigins { get; set; } = Array.Empty<string>();

    /// <summary>
    /// Rate limiting configuration
    /// </summary>
    public RateLimitOptions RateLimit { get; set; } = new();
}

/// <summary>
/// Rate limiting configuration
/// </summary>
public class RateLimitOptions
{
    /// <summary>
    /// Maximum requests per minute
    /// </summary>
    public int MaxRequestsPerMinute { get; set; } = 100;

    /// <summary>
    /// Maximum requests per hour
    /// </summary>
    public int MaxRequestsPerHour { get; set; } = 1000;

    /// <summary>
    /// Maximum requests per day
    /// </summary>
    public int MaxRequestsPerDay { get; set; } = 10000;

    /// <summary>
    /// Whether to enable rate limiting
    /// </summary>
    public bool Enabled { get; set; } = true;
}
