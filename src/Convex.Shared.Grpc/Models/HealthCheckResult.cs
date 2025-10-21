namespace Convex.Shared.Grpc.Models;

/// <summary>
/// Simple health check result
/// </summary>
public class HealthCheckResult
{
    /// <summary>
    /// Whether the service is healthy
    /// </summary>
    public bool IsHealthy { get; set; }
    
    /// <summary>
    /// Health check timestamp
    /// </summary>
    public DateTime Timestamp { get; set; }
    
    /// <summary>
    /// Service name
    /// </summary>
    public string ServiceName { get; set; } = string.Empty;
    
    /// <summary>
    /// Service version
    /// </summary>
    public string Version { get; set; } = string.Empty;
    
    /// <summary>
    /// Environment
    /// </summary>
    public string Environment { get; set; } = string.Empty;
    
    /// <summary>
    /// Whether the service is responsive
    /// </summary>
    public bool IsResponsive { get; set; }
    
    /// <summary>
    /// Health score (0-100)
    /// </summary>
    public int HealthScore { get; set; }
    
    /// <summary>
    /// Error message if unhealthy
    /// </summary>
    public string? ErrorMessage { get; set; }
}
