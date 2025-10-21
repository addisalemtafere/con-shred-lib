using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace Convex.Shared.Http.Services;

/// <summary>
/// HTTP client diagnostics and monitoring service
/// </summary>
public interface IHttpClientDiagnostics
{
    /// <summary>
    /// Gets current performance metrics
    /// </summary>
    HttpClientMetrics GetMetrics();

    /// <summary>
    /// Resets performance metrics
    /// </summary>
    void ResetMetrics();

    /// <summary>
    /// Records a successful request
    /// </summary>
    void RecordSuccess(string method, string uri, long elapsedMs, int statusCode);

    /// <summary>
    /// Records a failed request
    /// </summary>
    void RecordFailure(string method, string uri, long elapsedMs, Exception exception);
}

/// <summary>
/// HTTP client metrics
/// </summary>
public class HttpClientMetrics
{
    /// <summary>
    /// Total number of requests
    /// </summary>
    public long TotalRequests { get; set; }

    /// <summary>
    /// Total number of successful requests
    /// </summary>
    public long SuccessfulRequests { get; set; }

    /// <summary>
    /// Total number of failed requests
    /// </summary>
    public long FailedRequests { get; set; }

    /// <summary>
    /// Average response time in milliseconds
    /// </summary>
    public double AverageResponseTime { get; set; }

    /// <summary>
    /// Total response time in milliseconds
    /// </summary>
    public long TotalResponseTime { get; set; }

    /// <summary>
    /// Success rate percentage
    /// </summary>
    public double SuccessRate => TotalRequests > 0 ? (double)SuccessfulRequests / TotalRequests * 100 : 0;

    /// <summary>
    /// Requests per minute
    /// </summary>
    public double RequestsPerMinute { get; set; }

    /// <summary>
    /// Last request timestamp
    /// </summary>
    public DateTime LastRequestTime { get; set; }
}

/// <summary>
/// Implementation of HTTP client diagnostics
/// </summary>
public class HttpClientDiagnostics : IHttpClientDiagnostics
{
    private readonly ILogger<HttpClientDiagnostics> _logger;
    private readonly ConvexHttpClientOptions _options;
    private readonly object _lock = new();
    private HttpClientMetrics _metrics = new();
    private readonly DateTime _startTime = DateTime.UtcNow;

    public HttpClientDiagnostics(
        ILogger<HttpClientDiagnostics> logger,
        IOptions<ConvexHttpClientOptions> options)
    {
        _logger = logger;
        _options = options.Value;
    }

    public HttpClientMetrics GetMetrics()
    {
        lock (_lock)
        {
            var now = DateTime.UtcNow;
            var elapsedMinutes = (now - _startTime).TotalMinutes;
            
            return new HttpClientMetrics
            {
                TotalRequests = _metrics.TotalRequests,
                SuccessfulRequests = _metrics.SuccessfulRequests,
                FailedRequests = _metrics.FailedRequests,
                AverageResponseTime = _metrics.TotalRequests > 0 ? (double)_metrics.TotalResponseTime / _metrics.TotalRequests : 0,
                TotalResponseTime = _metrics.TotalResponseTime,
                RequestsPerMinute = elapsedMinutes > 0 ? _metrics.TotalRequests / elapsedMinutes : 0,
                LastRequestTime = _metrics.LastRequestTime
            };
        }
    }

    public void ResetMetrics()
    {
        lock (_lock)
        {
            _metrics = new HttpClientMetrics();
            _logger.LogInformation("HTTP client metrics reset");
        }
    }

    public void RecordSuccess(string method, string uri, long elapsedMs, int statusCode)
    {
        lock (_lock)
        {
            _metrics.TotalRequests++;
            _metrics.SuccessfulRequests++;
            _metrics.TotalResponseTime += elapsedMs;
            _metrics.LastRequestTime = DateTime.UtcNow;

            if (_options.EnableMetrics)
            {
                _logger.LogInformation("HTTP {Method} {Uri} succeeded in {ElapsedMs}ms with status {StatusCode}", 
                    method, uri, elapsedMs, statusCode);
            }
        }
    }

    public void RecordFailure(string method, string uri, long elapsedMs, Exception exception)
    {
        lock (_lock)
        {
            _metrics.TotalRequests++;
            _metrics.FailedRequests++;
            _metrics.TotalResponseTime += elapsedMs;
            _metrics.LastRequestTime = DateTime.UtcNow;

            _logger.LogError(exception, "HTTP {Method} {Uri} failed after {ElapsedMs}ms", 
                method, uri, elapsedMs);
        }
    }
}
