namespace Convex.Shared.Grpc.Models;

/// <summary>
/// Simple liveness probe result
/// </summary>
public class LivenessProbeResult
{
    /// <summary>
    /// Whether the service is alive
    /// </summary>
    public bool IsAlive { get; set; }
    
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
