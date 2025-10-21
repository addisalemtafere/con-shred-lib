using Convex.Shared.Grpc.Configuration;
using Convex.Shared.Grpc.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace Convex.Shared.Grpc.Services;

/// <summary>
/// Simple service manager for gRPC services
/// </summary>
public class ConvexGrpcServiceManager : IDisposable
{
    private readonly ILogger<ConvexGrpcServiceManager> _logger;
    private readonly ConvexGrpcOptions _options;
    private readonly ConcurrentDictionary<string, List<ServiceInstance>> _serviceInstances = new();
    private bool _disposed = false;

    /// <summary>
    /// Initializes a new instance of the ConvexGrpcServiceManager
    /// </summary>
    /// <param name="logger">Logger instance</param>
    /// <param name="options">gRPC configuration options</param>
    public ConvexGrpcServiceManager(
        ILogger<ConvexGrpcServiceManager> logger,
        IOptions<ConvexGrpcOptions> options)
    {
        _logger = logger;
        _options = options.Value;
        _logger.LogInformation("ConvexGrpcServiceManager initialized");
    }

    /// <summary>
    /// Register a service instance
    /// </summary>
    public void RegisterServiceInstance(string serviceName, ServiceInstance instance)
    {
        var instances = _serviceInstances.GetOrAdd(serviceName, _ => new List<ServiceInstance>());
        instances.Add(instance);
        _logger.LogInformation("Registered instance {InstanceId} for service {ServiceName}", 
            instance.InstanceId, serviceName);
    }

    /// <summary>
    /// Unregister a service instance
    /// </summary>
    public void UnregisterServiceInstance(string serviceName, string instanceId)
    {
        if (_serviceInstances.TryGetValue(serviceName, out var instances))
        {
            instances.RemoveAll(i => i.InstanceId == instanceId);
            _logger.LogInformation("Unregistered instance {InstanceId} for service {ServiceName}", 
                instanceId, serviceName);
        }
    }

    /// <summary>
    /// Get the best available service instance
    /// </summary>
    public ServiceInstance? GetBestServiceInstance(string serviceName)
    {
        if (!_serviceInstances.TryGetValue(serviceName, out var instances) || !instances.Any())
        {
            _logger.LogWarning("No instances available for service {ServiceName}", serviceName);
            return null;
        }

        var healthyInstances = instances.Where(i => i.IsHealthy).ToList();
        if (!healthyInstances.Any())
        {
            _logger.LogWarning("No healthy instances available for service {ServiceName}", serviceName);
            return null;
        }

        // Return the first healthy instance (simple selection)
        return healthyInstances.First();
    }

    /// <summary>
    /// Get all instances for a service
    /// </summary>
    public List<ServiceInstance> GetAllInstances(string serviceName)
    {
        return _serviceInstances.GetValueOrDefault(serviceName, new List<ServiceInstance>());
    }

    /// <summary>
    /// Dispose the service manager
    /// </summary>
    public void Dispose()
    {
        if (!_disposed)
        {
            _disposed = true;
            _logger.LogInformation("ConvexGrpcServiceManager disposed");
        }
    }
}
