using Convex.Shared.Logging.Interfaces;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Convex.Shared.Logging.Services;

/// <summary>
/// Enhanced logger implementation for Convex microservices
/// </summary>
public class ConvexLogger : IConvexLogger
{
    private readonly ILogger<ConvexLogger> _logger;
    private readonly string _serviceName;
    private readonly string _version;

    public ConvexLogger(ILogger<ConvexLogger> logger, string serviceName = "ConvexService", string version = "1.0.0")
    {
        _logger = logger;
        _serviceName = serviceName;
        _version = version;
    }

    public void LogInformation(string message, params object[] properties)
    {
        _logger.LogInformation(message, PrependStandardProperties(properties));
    }

    public void LogWarning(string message, params object[] properties)
    {
        _logger.LogWarning(message, PrependStandardProperties(properties));
    }

    public void LogError(string message, Exception? exception = null, params object[] properties)
    {
        if (exception != null)
        {
            _logger.LogError(exception, message, PrependStandardProperties(properties));
        }
        else
        {
            _logger.LogError(message, PrependStandardProperties(properties));
        }
    }

    public void LogDebug(string message, params object[] properties)
    {
        _logger.LogDebug(message, PrependStandardProperties(properties));
    }

    public void LogTrace(string message, params object[] properties)
    {
        _logger.LogTrace(message, PrependStandardProperties(properties));
    }

    public void LogPerformance(string operation, TimeSpan duration, params object[] properties)
    {
        var performanceData = new
        {
            Operation = operation,
            Duration = duration.TotalMilliseconds,
            DurationMs = (long)duration.TotalMilliseconds,
            Timestamp = DateTime.UtcNow
        };

        var allProperties = PrependStandardProperties(properties);
        _logger.LogInformation("Performance: {Operation} completed in {Duration}ms", 
            operation, duration.TotalMilliseconds, allProperties);
    }

    public void LogBusinessEvent(string eventName, object data, params object[] properties)
    {
        var eventData = new
        {
            EventName = eventName,
            Data = data,
            Timestamp = DateTime.UtcNow
        };

        var allProperties = PrependStandardProperties(properties);
        _logger.LogInformation("Business Event: {EventName} - {Data}", 
            eventName, data, allProperties);
    }

    public void LogApiRequest(string method, string url, int statusCode, TimeSpan duration, params object[] properties)
    {
        var requestData = new
        {
            Method = method,
            Url = url,
            StatusCode = statusCode,
            Duration = duration.TotalMilliseconds,
            Timestamp = DateTime.UtcNow
        };

        var logLevel = statusCode >= 400 ? LogLevel.Warning : LogLevel.Information;
        var allProperties = PrependStandardProperties(properties);
        _logger.Log(logLevel, "API Request: {Method} {Url} - {StatusCode} in {Duration}ms", 
            method, url, statusCode, duration.TotalMilliseconds, allProperties);
    }

    public void LogWithCorrelation(string correlationId, string message, params object[] properties)
    {
        var correlationProperties = new object[] { correlationId }.Concat(properties).ToArray();
        _logger.LogInformation(message, PrependStandardProperties(correlationProperties));
    }

    private object[] PrependStandardProperties(object[] properties)
    {
        var standardProperties = new object[]
        {
            _serviceName,
            _version,
            Environment.MachineName,
            Process.GetCurrentProcess().Id
        };

        return standardProperties.Concat(properties).ToArray();
    }
}
