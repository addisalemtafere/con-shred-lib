namespace Convex.Shared.Grpc.Models;

/// <summary>
/// Simple service instance information
/// </summary>
public class ServiceInstance
{
    /// <summary>
    /// Instance identifier
    /// </summary>
    public string InstanceId { get; set; } = string.Empty;
    
    /// <summary>
    /// Instance endpoint
    /// </summary>
    public string Endpoint { get; set; } = string.Empty;
    
    /// <summary>
    /// Whether the instance is healthy
    /// </summary>
    public bool IsHealthy { get; set; } = true;
    
    /// <summary>
    /// Instance creation timestamp
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
