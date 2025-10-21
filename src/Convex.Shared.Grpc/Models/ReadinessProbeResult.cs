namespace Convex.Shared.Grpc.Models;

/// <summary>
/// Simple readiness probe result
/// </summary>
public class ReadinessProbeResult
{
    /// <summary>
    /// Whether the service is ready
    /// </summary>
    public bool IsReady { get; set; }
    
    /// <summary>
    /// Check timestamp
    /// </summary>
    public DateTime Timestamp { get; set; }
    
    /// <summary>
    /// Service name
    /// </summary>
    public string ServiceName { get; set; } = string.Empty;
    
    /// <summary>
    /// Health score
    /// </summary>
    public int HealthScore { get; set; }
}
