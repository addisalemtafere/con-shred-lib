namespace Convex.Shared.Http;

/// <summary>
/// Configuration options for Convex HTTP client
/// </summary>
public class ConvexHttpClientOptions
{
    /// <summary>
    /// The base address for the HTTP client
    /// </summary>
    public Uri? BaseAddress { get; set; }

    /// <summary>
    /// The timeout for HTTP requests (default: 30 seconds)
    /// </summary>
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// The API key to include in requests
    /// </summary>
    public string? ApiKey { get; set; }

    /// <summary>
    /// The bearer token to include in requests
    /// </summary>
    public string? BearerToken { get; set; }

    /// <summary>
    /// Additional headers to include in all requests
    /// </summary>
    public Dictionary<string, string> DefaultHeaders { get; set; } = new();

    /// <summary>
    /// Whether to enable request/response logging
    /// </summary>
    public bool EnableLogging { get; set; } = true;

    /// <summary>
    /// Whether to enable performance metrics
    /// </summary>
    public bool EnableMetrics { get; set; } = true;

    /// <summary>
    /// Whether to enable detailed debugging information
    /// </summary>
    public bool EnableDebugLogging { get; set; } = false;

    /// <summary>
    /// Whether to log request/response content (be careful with sensitive data)
    /// </summary>
    public bool EnableContentLogging { get; set; } = false;

    /// <summary>
    /// Maximum content length to log (default: 1024 characters)
    /// </summary>
    public int MaxContentLogLength { get; set; } = 1024;

    /// <summary>
    /// Retry policy configuration
    /// </summary>
    public RetryPolicyOptions RetryPolicy { get; set; } = new();

    /// <summary>
    /// Circuit breaker configuration
    /// </summary>
    public CircuitBreakerOptions CircuitBreaker { get; set; } = new();

    /// <summary>
    /// Timeout policy configuration
    /// </summary>
    public TimeoutPolicyOptions TimeoutPolicy { get; set; } = new();

    /// <summary>
    /// Authentication token management configuration
    /// </summary>
    public AuthenticationOptions Authentication { get; set; } = new();
}

/// <summary>
/// Retry policy configuration
/// </summary>
public class RetryPolicyOptions
{
    /// <summary>
    /// Maximum number of retry attempts (default: 3)
    /// </summary>
    public int MaxRetryAttempts { get; set; } = 3;

    /// <summary>
    /// Base delay between retries (default: 1 second)
    /// </summary>
    public TimeSpan BaseDelay { get; set; } = TimeSpan.FromSeconds(1);

    /// <summary>
    /// Maximum delay between retries (default: 30 seconds)
    /// </summary>
    public TimeSpan MaxDelay { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Whether to use exponential backoff (default: true)
    /// </summary>
    public bool UseExponentialBackoff { get; set; } = true;

    /// <summary>
    /// Jitter factor for randomization (default: 0.1)
    /// </summary>
    public double JitterFactor { get; set; } = 0.1;

    /// <summary>
    /// HTTP status codes to retry on
    /// </summary>
    public int[] RetryableStatusCodes { get; set; } = { 408, 429, 500, 502, 503, 504 };
}

/// <summary>
/// Circuit breaker configuration
/// </summary>
public class CircuitBreakerOptions
{
    /// <summary>
    /// Number of consecutive failures before opening circuit (default: 5)
    /// </summary>
    public int FailureThreshold { get; set; } = 5;

    /// <summary>
    /// Duration to keep circuit open (default: 30 seconds)
    /// </summary>
    public TimeSpan DurationOfBreak { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Minimum throughput before circuit breaker can be triggered (default: 10)
    /// </summary>
    public int MinimumThroughput { get; set; } = 10;

    /// <summary>
    /// Whether to enable circuit breaker (default: true)
    /// </summary>
    public bool Enabled { get; set; } = true;
}

/// <summary>
/// Timeout policy configuration
/// </summary>
public class TimeoutPolicyOptions
{
    /// <summary>
    /// Timeout duration (default: 30 seconds)
    /// </summary>
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Whether to enable timeout policy (default: true)
    /// </summary>
    public bool Enabled { get; set; } = true;
}

/// <summary>
/// Authentication types supported
/// </summary>
public static class AuthenticationTypes
{
    /// <summary>
    /// No authentication
    /// </summary>
    public const string None = "None";
    
    /// <summary>
    /// API Key authentication
    /// </summary>
    public const string ApiKey = "ApiKey";
    
    /// <summary>
    /// Bearer token authentication
    /// </summary>
    public const string BearerToken = "BearerToken";
    
    /// <summary>
    /// Basic authentication
    /// </summary>
    public const string Basic = "Basic";
    
    /// <summary>
    /// OAuth2 client credentials flow
    /// </summary>
    public const string OAuth2ClientCredentials = "OAuth2ClientCredentials";
    
    /// <summary>
    /// Custom authentication
    /// </summary>
    public const string Custom = "Custom";
}

/// <summary>
/// Authentication configuration
/// </summary>
public class AuthenticationOptions
{
    /// <summary>
    /// Authentication type (default: None)
    /// </summary>
    public string Type { get; set; } = AuthenticationTypes.None;

    /// <summary>
    /// Whether to enable automatic token management (default: false)
    /// </summary>
    public bool EnableTokenManagement { get; set; } = false;

    // API Key Authentication
    /// <summary>
    /// API Key for authentication
    /// </summary>
    public string? ApiKey { get; set; }

    /// <summary>
    /// API Key header name (default: X-API-Key)
    /// </summary>
    public string ApiKeyHeaderName { get; set; } = "X-API-Key";

    // Bearer Token Authentication
    /// <summary>
    /// Bearer token for authentication
    /// </summary>
    public string? BearerToken { get; set; }

    // Basic Authentication
    /// <summary>
    /// Username for basic authentication
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// Password for basic authentication
    /// </summary>
    public string? Password { get; set; }

    // OAuth2 Client Credentials
    /// <summary>
    /// Token endpoint URL for OAuth2 authentication
    /// </summary>
    public string? TokenEndpoint { get; set; }

    /// <summary>
    /// Client ID for OAuth2 authentication
    /// </summary>
    public string? ClientId { get; set; }

    /// <summary>
    /// Client secret for OAuth2 authentication
    /// </summary>
    public string? ClientSecret { get; set; }

    /// <summary>
    /// OAuth2 scope
    /// </summary>
    public string? Scope { get; set; }

    /// <summary>
    /// Token cache duration (default: 50 minutes)
    /// </summary>
    public TimeSpan TokenCacheDuration { get; set; } = TimeSpan.FromMinutes(50);

    /// <summary>
    /// Token refresh buffer time (default: 5 minutes before expiry)
    /// </summary>
    public TimeSpan TokenRefreshBuffer { get; set; } = TimeSpan.FromMinutes(5);

    /// <summary>
    /// Additional authentication parameters
    /// </summary>
    public Dictionary<string, string> AdditionalParameters { get; set; } = new();

    /// <summary>
    /// Custom authentication header name
    /// </summary>
    public string? CustomHeaderName { get; set; }

    /// <summary>
    /// Custom authentication header value
    /// </summary>
    public string? CustomHeaderValue { get; set; }
}