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
}