using Convex.Shared.Grpc.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace Convex.Shared.Grpc.Services;

/// <summary>
/// High-performance metrics collection for gRPC services
/// </summary>
public class ConvexGrpcMetrics
{
    private readonly ILogger<ConvexGrpcMetrics> _logger;
    private readonly ConvexGrpcOptions _options;
    private readonly ConcurrentDictionary<string, ServiceMetrics> _serviceMetrics = new();
    private readonly object _lock = new();

    /// <summary>
    /// Service metrics data
    /// </summary>
    private class ServiceMetrics
    {
        public long TotalRequests { get; set; } = 0;
        public long SuccessfulRequests { get; set; } = 0;
        public long FailedRequests { get; set; } = 0;
        public long TotalResponseTimeMs { get; set; } = 0;
        public long MinResponseTimeMs { get; set; } = long.MaxValue;
        public long MaxResponseTimeMs { get; set; } = 0;
        public DateTime LastRequestTime { get; set; } = DateTime.MinValue;
        public DateTime FirstRequestTime { get; set; } = DateTime.MaxValue;
        public long ActiveConnections { get; set; } = 0;
        public long TotalBytesTransferred { get; set; } = 0;
    }

    /// <summary>
    /// Initializes a new instance of the ConvexGrpcMetrics
    /// </summary>
    /// <param name="logger">Logger instance</param>
    /// <param name="options">gRPC configuration options</param>
    public ConvexGrpcMetrics(
        ILogger<ConvexGrpcMetrics> logger,
        IOptions<ConvexGrpcOptions> options)
    {
        _logger = logger;
        _options = options.Value;
    }

    /// <summary>
    /// Record a request start
    /// </summary>
    /// <param name="serviceName">Service name</param>
    /// <param name="requestSize">Request size in bytes</param>
    /// <returns>Request ID for tracking</returns>
    public string RecordRequestStart(string serviceName, long requestSize = 0)
    {
        if (!_options.EnableMetrics)
            return string.Empty;

        var requestId = Guid.NewGuid().ToString();
        var metrics = _serviceMetrics.GetOrAdd(serviceName, _ => new ServiceMetrics());

        lock (_lock)
        {
            metrics.TotalRequests++;
            metrics.ActiveConnections++;
            metrics.TotalBytesTransferred += requestSize;
            metrics.LastRequestTime = DateTime.UtcNow;
            
            if (metrics.FirstRequestTime == DateTime.MaxValue)
                metrics.FirstRequestTime = DateTime.UtcNow;
        }

        _logger.LogDebug("Started request {RequestId} for service {ServiceName}", requestId, serviceName);
        return requestId;
    }

    /// <summary>
    /// Record a successful request completion
    /// </summary>
    /// <param name="serviceName">Service name</param>
    /// <param name="requestId">Request ID</param>
    /// <param name="responseTimeMs">Response time in milliseconds</param>
    /// <param name="responseSize">Response size in bytes</param>
    public void RecordRequestSuccess(string serviceName, string requestId, long responseTimeMs, long responseSize = 0)
    {
        if (!_options.EnableMetrics)
            return;

        var metrics = _serviceMetrics.GetOrAdd(serviceName, _ => new ServiceMetrics());

        lock (_lock)
        {
            metrics.SuccessfulRequests++;
            metrics.ActiveConnections = Math.Max(0, metrics.ActiveConnections - 1);
            metrics.TotalResponseTimeMs += responseTimeMs;
            metrics.TotalBytesTransferred += responseSize;

            // Update min/max response times
            if (responseTimeMs < metrics.MinResponseTimeMs)
                metrics.MinResponseTimeMs = responseTimeMs;
            if (responseTimeMs > metrics.MaxResponseTimeMs)
                metrics.MaxResponseTimeMs = responseTimeMs;
        }

        _logger.LogDebug("Completed request {RequestId} for service {ServiceName} in {ResponseTime}ms", 
            requestId, serviceName, responseTimeMs);
    }

    /// <summary>
    /// Record a failed request
    /// </summary>
    /// <param name="serviceName">Service name</param>
    /// <param name="requestId">Request ID</param>
    /// <param name="responseTimeMs">Response time in milliseconds</param>
    /// <param name="error">Error message</param>
    public void RecordRequestFailure(string serviceName, string requestId, long responseTimeMs, string error)
    {
        if (!_options.EnableMetrics)
            return;

        var metrics = _serviceMetrics.GetOrAdd(serviceName, _ => new ServiceMetrics());

        lock (_lock)
        {
            metrics.FailedRequests++;
            metrics.ActiveConnections = Math.Max(0, metrics.ActiveConnections - 1);
            metrics.TotalResponseTimeMs += responseTimeMs;

            // Update min/max response times
            if (responseTimeMs < metrics.MinResponseTimeMs)
                metrics.MinResponseTimeMs = responseTimeMs;
            if (responseTimeMs > metrics.MaxResponseTimeMs)
                metrics.MaxResponseTimeMs = responseTimeMs;
        }

        _logger.LogWarning("Failed request {RequestId} for service {ServiceName} in {ResponseTime}ms: {Error}", 
            requestId, serviceName, responseTimeMs, error);
    }

    /// <summary>
    /// Get metrics for a specific service
    /// </summary>
    /// <param name="serviceName">Service name</param>
    /// <returns>Service metrics</returns>
    public object GetServiceMetrics(string serviceName)
    {
        if (!_serviceMetrics.TryGetValue(serviceName, out var metrics))
        {
            return new { ServiceName = serviceName, Message = "No metrics available" };
        }

        var avgResponseTime = metrics.TotalRequests > 0 ? metrics.TotalResponseTimeMs / metrics.TotalRequests : 0;
        var successRate = metrics.TotalRequests > 0 ? (double)metrics.SuccessfulRequests / metrics.TotalRequests * 100 : 0;
        var uptime = metrics.FirstRequestTime != DateTime.MaxValue ? DateTime.UtcNow - metrics.FirstRequestTime : TimeSpan.Zero;

        return new
        {
            ServiceName = serviceName,
            TotalRequests = metrics.TotalRequests,
            SuccessfulRequests = metrics.SuccessfulRequests,
            FailedRequests = metrics.FailedRequests,
            SuccessRate = Math.Round(successRate, 2),
            AverageResponseTimeMs = avgResponseTime,
            MinResponseTimeMs = metrics.MinResponseTimeMs == long.MaxValue ? 0 : metrics.MinResponseTimeMs,
            MaxResponseTimeMs = metrics.MaxResponseTimeMs,
            ActiveConnections = metrics.ActiveConnections,
            TotalBytesTransferred = metrics.TotalBytesTransferred,
            Uptime = uptime.ToString(@"dd\.hh\:mm\:ss"),
            LastRequestTime = metrics.LastRequestTime,
            RequestsPerSecond = CalculateRequestsPerSecond(metrics)
        };
    }

    /// <summary>
    /// Get all service metrics
    /// </summary>
    /// <returns>All service metrics</returns>
    public Dictionary<string, object> GetAllMetrics()
    {
        var allMetrics = new Dictionary<string, object>();
        
        foreach (var service in _serviceMetrics.Keys)
        {
            allMetrics[service] = GetServiceMetrics(service);
        }
        
        return allMetrics;
    }

    /// <summary>
    /// Get system-wide metrics summary
    /// </summary>
    /// <returns>System metrics summary</returns>
    public object GetSystemMetrics()
    {
        var totalRequests = _serviceMetrics.Values.Sum(m => m.TotalRequests);
        var totalSuccessful = _serviceMetrics.Values.Sum(m => m.SuccessfulRequests);
        var totalFailed = _serviceMetrics.Values.Sum(m => m.FailedRequests);
        var totalResponseTime = _serviceMetrics.Values.Sum(m => m.TotalResponseTimeMs);
        var totalBytes = _serviceMetrics.Values.Sum(m => m.TotalBytesTransferred);
        var activeConnections = _serviceMetrics.Values.Sum(m => m.ActiveConnections);

        var overallSuccessRate = totalRequests > 0 ? (double)totalSuccessful / totalRequests * 100 : 0;
        var avgResponseTime = totalRequests > 0 ? totalResponseTime / totalRequests : 0;

        return new
        {
            TotalServices = _serviceMetrics.Count,
            TotalRequests = totalRequests,
            SuccessfulRequests = totalSuccessful,
            FailedRequests = totalFailed,
            OverallSuccessRate = Math.Round(overallSuccessRate, 2),
            AverageResponseTimeMs = avgResponseTime,
            ActiveConnections = activeConnections,
            TotalBytesTransferred = totalBytes,
            Timestamp = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Reset metrics for a service
    /// </summary>
    /// <param name="serviceName">Service name</param>
    public void ResetMetrics(string serviceName)
    {
        if (_serviceMetrics.TryGetValue(serviceName, out var metrics))
        {
            lock (_lock)
            {
                metrics.TotalRequests = 0;
                metrics.SuccessfulRequests = 0;
                metrics.FailedRequests = 0;
                metrics.TotalResponseTimeMs = 0;
                metrics.MinResponseTimeMs = long.MaxValue;
                metrics.MaxResponseTimeMs = 0;
                metrics.ActiveConnections = 0;
                metrics.TotalBytesTransferred = 0;
                metrics.FirstRequestTime = DateTime.MaxValue;
                metrics.LastRequestTime = DateTime.MinValue;
            }
            
            _logger.LogInformation("Reset metrics for service {ServiceName}", serviceName);
        }
    }

    /// <summary>
    /// Reset all metrics
    /// </summary>
    public void ResetAllMetrics()
    {
        lock (_lock)
        {
            _serviceMetrics.Clear();
        }
        
        _logger.LogInformation("Reset all metrics");
    }

    private double CalculateRequestsPerSecond(ServiceMetrics metrics)
    {
        if (metrics.FirstRequestTime == DateTime.MaxValue || metrics.TotalRequests == 0)
            return 0;

        var uptime = DateTime.UtcNow - metrics.FirstRequestTime;
        return uptime.TotalSeconds > 0 ? metrics.TotalRequests / uptime.TotalSeconds : 0;
    }
}
