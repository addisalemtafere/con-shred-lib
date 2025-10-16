using Convex.Shared.Logging.Interfaces;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Convex.Shared.Logging.Services;

/// <summary>
/// High-performance logger optimized for billion-record scenarios
/// </summary>
public class ConvexLogger : IConvexLogger
{
    private readonly ILogger<ConvexLogger> _logger;
    private readonly string _serviceName;
    private readonly string _version;
    private readonly int _processId;
    private readonly string _machineName;

    public ConvexLogger(
        ILogger<ConvexLogger> logger,
        string serviceName = "ConvexService",
        string version = "1.0.0")
    {
        _logger = logger;
        _serviceName = serviceName;
        _version = version;
        _processId = Process.GetCurrentProcess().Id;
        _machineName = Environment.MachineName;
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
            allProperties);
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
            allProperties);
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
            allProperties);
    }

    public void LogWithCorrelation(string correlationId, string message, params object[] properties)
    {
        var correlationProperties = new object[] { correlationId }.Concat(properties).ToArray();
        _logger.LogInformation(message, PrependStandardProperties(correlationProperties));
    }

    public void LogBatch(params (string message, object[] properties)[] messages)
    {
        // High-performance batch logging for billion-record scenarios
        foreach (var (message, properties) in messages)
        {
            _logger.LogInformation(message, PrependStandardProperties(properties));
        }
    }

    public void LogException(Exception exception, string message, params object[] context)
    {
        var properties = PrependStandardProperties(context);
        _logger.LogError(exception, message, properties);
    }

    public void LogBusinessException(Exception exception, string? correlationId = null, string? userId = null, string? requestId = null)
    {
        var context = new List<object>();

        if (!string.IsNullOrEmpty(correlationId))
            context.AddRange(new object[] { "CorrelationId", correlationId });

        if (!string.IsNullOrEmpty(userId))
            context.AddRange(new object[] { "UserId", userId });

        if (!string.IsNullOrEmpty(requestId))
            context.AddRange(new object[] { "RequestId", requestId });

        var properties = PrependStandardProperties(context.ToArray());
        _logger.LogError(exception, "Business exception occurred: {ExceptionType}", exception.GetType().Name);
    }

    public void LogValidationErrors(object[] validationErrors, string? correlationId = null, string? userId = null)
    {
        var context = new List<object>();

        if (!string.IsNullOrEmpty(correlationId))
            context.AddRange(new object[] { "CorrelationId", correlationId });

        if (!string.IsNullOrEmpty(userId))
            context.AddRange(new object[] { "UserId", userId });

        context.AddRange(new object[] { "ValidationErrors", validationErrors });

        var properties = PrependStandardProperties(context.ToArray());
        _logger.LogWarning("Validation errors occurred: {ErrorCount} errors", validationErrors.Length);
    }

    private object[] PrependStandardProperties(object[] properties)
    {
        // Pre-allocate array for better performance with high-volume logging
        var result = new object[4 + properties.Length];
        result[0] = _serviceName;
        result[1] = _version;
        result[2] = _machineName;
        result[3] = _processId;

        // Copy properties array for better performance
        Array.Copy(properties, 0, result, 4, properties.Length);

        return result;
    }
}