namespace Convex.Shared.Grpc.Models;

/// <summary>
/// Simple service endpoint information
/// </summary>
public class ServiceEndpoint
{
    /// <summary>
    /// Service endpoint URL
    /// </summary>
    public string Endpoint { get; set; } = string.Empty;
    
    /// <summary>
    /// Service name
    /// </summary>
    public string ServiceName { get; set; } = string.Empty;
    
    /// <summary>
    /// Whether the endpoint is healthy
    /// </summary>
    public bool IsHealthy { get; set; } = true;
    
    /// <summary>
    /// Last health check timestamp
    /// </summary>
    public DateTime LastHealthCheck { get; set; } = DateTime.UtcNow;
}
