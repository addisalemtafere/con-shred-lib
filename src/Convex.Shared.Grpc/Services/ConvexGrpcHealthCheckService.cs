using Convex.Shared.Grpc.Configuration;
using Convex.Shared.Grpc.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Convex.Shared.Grpc.Services;

/// <summary>
/// Simple health check service for Kubernetes probes
/// </summary>
public class ConvexGrpcHealthCheckService : IDisposable
{
    private readonly ILogger<ConvexGrpcHealthCheckService> _logger;
    private readonly ConvexGrpcOptions _options;
    private bool _disposed = false;

    /// <summary>
    /// Initializes a new instance of the ConvexGrpcHealthCheckService
    /// </summary>
    /// <param name="logger">Logger instance</param>
    /// <param name="options">gRPC configuration options</param>
    public ConvexGrpcHealthCheckService(
        ILogger<ConvexGrpcHealthCheckService> logger,
        IOptions<ConvexGrpcOptions> options)
    {
        _logger = logger;
        _options = options.Value;
        _logger.LogInformation("ConvexGrpcHealthCheckService initialized");
    }

    /// <summary>
    /// Get basic health status
    /// </summary>
    public HealthCheckResult GetHealthStatus()
    {
        var result = new HealthCheckResult
        {
            IsHealthy = true,
            Timestamp = DateTime.UtcNow,
            ServiceName = _options.ServiceName ?? "convex-grpc-service",
            Version = "1.0.0",
            Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"
        };

        try
        {
            // Simple health check - just check if service is responsive
            result.IsResponsive = CheckServiceResponsiveness();
            result.IsHealthy = result.IsResponsive;
            result.HealthScore = result.IsHealthy ? 100 : 0;

            _logger.LogDebug("Health check completed: Healthy={IsHealthy}, Responsive={IsResponsive}", 
                result.IsHealthy, result.IsResponsive);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during health check");
            result.IsHealthy = false;
            result.HealthScore = 0;
            result.ErrorMessage = ex.Message;
        }

        return result;
    }

    /// <summary>
    /// Get liveness probe result for Kubernetes
    /// </summary>
    public LivenessProbeResult GetLivenessStatus()
    {
        var health = GetHealthStatus();
        
        return new LivenessProbeResult
        {
            IsAlive = health.IsHealthy && health.IsResponsive,
            Timestamp = health.Timestamp,
            ServiceName = health.ServiceName,
            HealthScore = health.HealthScore
        };
    }

    /// <summary>
    /// Get readiness probe result for Kubernetes
    /// </summary>
    public ReadinessProbeResult GetReadinessStatus()
    {
        var health = GetHealthStatus();
        
        return new ReadinessProbeResult
        {
            IsReady = health.IsHealthy,
            Timestamp = health.Timestamp,
            ServiceName = health.ServiceName,
            HealthScore = health.HealthScore
        };
    }

    private bool CheckServiceResponsiveness()
    {
        try
        {
            // Simple responsiveness check
            return Environment.ProcessId > 0;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Dispose the health check service
    /// </summary>
    public void Dispose()
    {
        if (!_disposed)
        {
            _disposed = true;
            _logger.LogInformation("ConvexGrpcHealthCheckService disposed");
        }
    }
}
