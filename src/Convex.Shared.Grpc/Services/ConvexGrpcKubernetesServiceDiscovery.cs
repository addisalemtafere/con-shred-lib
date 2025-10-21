using Convex.Shared.Grpc.Configuration;
using Convex.Shared.Grpc.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace Convex.Shared.Grpc.Services;

/// <summary>
/// Simple Kubernetes service discovery for gRPC services
/// </summary>
public class ConvexGrpcKubernetesServiceDiscovery : IDisposable
{
    private readonly ILogger<ConvexGrpcKubernetesServiceDiscovery> _logger;
    private readonly ConvexGrpcOptions _options;
    private readonly ConcurrentDictionary<string, List<ServiceEndpoint>> _serviceEndpoints = new();
    private bool _disposed = false;

    /// <summary>
    /// Initializes a new instance of the ConvexGrpcKubernetesServiceDiscovery
    /// </summary>
    /// <param name="logger">Logger instance</param>
    /// <param name="options">gRPC configuration options</param>
    public ConvexGrpcKubernetesServiceDiscovery(
        ILogger<ConvexGrpcKubernetesServiceDiscovery> logger,
        IOptions<ConvexGrpcOptions> options)
    {
        _logger = logger;
        _options = options.Value;
        _logger.LogInformation("ConvexGrpcKubernetesServiceDiscovery initialized");
    }

    /// <summary>
    /// Get the best available service endpoint
    /// </summary>
    public ServiceEndpoint? GetServiceEndpoint(string serviceName)
    {
        if (!_serviceEndpoints.TryGetValue(serviceName, out var endpoints) || !endpoints.Any())
        {
            _logger.LogWarning("No endpoints available for service {ServiceName}", serviceName);
            return null;
        }

        var healthyEndpoints = endpoints.Where(e => e.IsHealthy).ToList();
        if (!healthyEndpoints.Any())
        {
            _logger.LogWarning("No healthy endpoints available for service {ServiceName}", serviceName);
            return null;
        }

        // Return the first healthy endpoint (simple selection)
        return healthyEndpoints.First();
    }

    /// <summary>
    /// Register a service endpoint
    /// </summary>
    public void RegisterServiceEndpoint(string serviceName, ServiceEndpoint endpoint)
    {
        var endpoints = _serviceEndpoints.GetOrAdd(serviceName, _ => new List<ServiceEndpoint>());
        endpoints.Add(endpoint);
        _logger.LogInformation("Registered endpoint {Endpoint} for service {ServiceName}", 
            endpoint.Endpoint, serviceName);
    }

    /// <summary>
    /// Update endpoint health status
    /// </summary>
    public void UpdateEndpointHealth(string serviceName, string endpoint, bool isHealthy)
    {
        if (_serviceEndpoints.TryGetValue(serviceName, out var endpoints))
        {
            var targetEndpoint = endpoints.FirstOrDefault(e => e.Endpoint == endpoint);
            if (targetEndpoint != null)
            {
                targetEndpoint.IsHealthy = isHealthy;
                targetEndpoint.LastHealthCheck = DateTime.UtcNow;
            }
        }
    }

    /// <summary>
    /// Get all endpoints for a service
    /// </summary>
    public List<ServiceEndpoint> GetAllServiceEndpoints(string serviceName)
    {
        return _serviceEndpoints.GetValueOrDefault(serviceName, new List<ServiceEndpoint>());
    }

    /// <summary>
    /// Dispose the service discovery
    /// </summary>
    public void Dispose()
    {
        if (!_disposed)
        {
            _disposed = true;
            _logger.LogInformation("ConvexGrpcKubernetesServiceDiscovery disposed");
        }
    }
}