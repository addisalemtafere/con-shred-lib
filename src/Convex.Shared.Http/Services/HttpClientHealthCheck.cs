using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Convex.Shared.Http.Services;

/// <summary>
/// HTTP client health check for production monitoring
/// </summary>
public class HttpClientHealthCheck : IHealthCheck
{
    private readonly IHttpClientDiagnostics _diagnostics;
    private readonly ILogger<HttpClientHealthCheck> _logger;
    private readonly ConvexHttpClientOptions _options;

    public HttpClientHealthCheck(
        IHttpClientDiagnostics diagnostics,
        ILogger<HttpClientHealthCheck> logger,
        IOptions<ConvexHttpClientOptions> options)
    {
        _diagnostics = diagnostics;
        _logger = logger;
        _options = options.Value;
    }

    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var metrics = _diagnostics.GetMetrics();
            var data = new Dictionary<string, object>
            {
                ["TotalRequests"] = metrics.TotalRequests,
                ["SuccessfulRequests"] = metrics.SuccessfulRequests,
                ["FailedRequests"] = metrics.FailedRequests,
                ["SuccessRate"] = $"{metrics.SuccessRate:F2}%",
                ["AverageResponseTime"] = $"{metrics.AverageResponseTime:F2}ms",
                ["RequestsPerMinute"] = $"{metrics.RequestsPerMinute:F2}",
                ["LastRequestTime"] = metrics.LastRequestTime.ToString("O")
            };

            // Determine health status based on metrics
            if (metrics.TotalRequests == 0)
            {
                return Task.FromResult(HealthCheckResult.Healthy("No requests made yet", data));
            }

            if (metrics.SuccessRate < 50)
            {
                _logger.LogWarning("HTTP client health check failed: Success rate {SuccessRate}% is below 50%", 
                    metrics.SuccessRate);
                return Task.FromResult(HealthCheckResult.Unhealthy(
                    $"Success rate {metrics.SuccessRate:F2}% is below 50%", 
                    null, 
                    data));
            }

            if (metrics.AverageResponseTime > 5000) // 5 seconds
            {
                _logger.LogWarning("HTTP client health check degraded: Average response time {AverageResponseTime}ms is above 5000ms", 
                    metrics.AverageResponseTime);
                return Task.FromResult(HealthCheckResult.Degraded(
                    $"Average response time {metrics.AverageResponseTime:F2}ms is above 5000ms", 
                    null, 
                    data));
            }

            _logger.LogDebug("HTTP client health check passed: Success rate {SuccessRate}%, Average response time {AverageResponseTime}ms", 
                metrics.SuccessRate, metrics.AverageResponseTime);

            return Task.FromResult(HealthCheckResult.Healthy(
                $"Success rate: {metrics.SuccessRate:F2}%, Average response time: {metrics.AverageResponseTime:F2}ms", 
                data));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "HTTP client health check failed with exception");
            return Task.FromResult(HealthCheckResult.Unhealthy("Health check failed with exception", ex));
        }
    }
}
