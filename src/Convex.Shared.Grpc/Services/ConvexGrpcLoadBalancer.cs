using Convex.Shared.Grpc.Configuration;
using Convex.Shared.Grpc.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace Convex.Shared.Grpc.Services;

/// <summary>
/// Simple gRPC client-side load balancer for Kubernetes
/// Solves the L4 load balancing issue with gRPC in Kubernetes
/// </summary>
public class ConvexGrpcLoadBalancer : IDisposable
{
    private readonly ILogger<ConvexGrpcLoadBalancer> _logger;
    private readonly ConvexGrpcOptions _options;
    private readonly ConcurrentDictionary<string, List<ServiceEndpoint>> _serviceEndpoints = new();
    private readonly ConcurrentDictionary<string, int> _roundRobinCounters = new();
    private bool _disposed = false;

    /// <summary>
    /// Initializes a new instance of the ConvexGrpcLoadBalancer
    /// </summary>
    /// <param name="logger">Logger instance</param>
    /// <param name="options">gRPC configuration options</param>
    public ConvexGrpcLoadBalancer(
        ILogger<ConvexGrpcLoadBalancer> logger,
        IOptions<ConvexGrpcOptions> options)
    {
        _logger = logger;
        _options = options.Value;
        _logger.LogInformation("ConvexGrpcLoadBalancer initialized");
    }

    /// <summary>
    /// Get the best available service endpoint using round-robin load balancing
    /// </summary>
    public ServiceEndpoint? GetLoadBalancedEndpoint(string serviceName)
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

        // Simple round-robin load balancing
        var counter = _roundRobinCounters.AddOrUpdate(serviceName, 0, (key, value) => (value + 1) % healthyEndpoints.Count);
        var selectedEndpoint = healthyEndpoints[counter];
        
        _logger.LogDebug("Selected endpoint {Endpoint} for service {ServiceName}", selectedEndpoint.Endpoint, serviceName);
        return selectedEndpoint;
    }

    /// <summary>
    /// Register a service endpoint
    /// </summary>
    public void RegisterEndpoint(string serviceName, ServiceEndpoint endpoint)
    {
        var endpoints = _serviceEndpoints.GetOrAdd(serviceName, _ => new List<ServiceEndpoint>());
        endpoints.Add(endpoint);
        _logger.LogInformation("Registered endpoint {Endpoint} for service {ServiceName}", endpoint.Endpoint, serviceName);
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
    public List<ServiceEndpoint> GetAllEndpoints(string serviceName)
    {
        return _serviceEndpoints.GetValueOrDefault(serviceName, new List<ServiceEndpoint>());
    }

    /// <summary>
    /// Dispose the load balancer
    /// </summary>
    public void Dispose()
    {
        if (!_disposed)
        {
            _disposed = true;
            _logger.LogInformation("ConvexGrpcLoadBalancer disposed");
        }
    }
}
