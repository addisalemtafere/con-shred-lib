using Convex.Shared.Logging.Interfaces;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Collections.Concurrent;

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
    
    // Performance optimization: Object pooling for high-volume scenarios
    private static readonly ConcurrentQueue<object[]> _propertyBufferPool = new();
    private static readonly ConcurrentQueue<object[]> _contextBufferPool = new();
    private const int MaxProperties = 20;
    private const int MaxContextSize = 10;

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
        // Performance optimization: Use direct logging instead of anonymous objects
        var durationMs = (long)duration.TotalMilliseconds;
        var allProperties = PrependStandardProperties(properties);
        _logger.LogInformation("Performance: {Operation} completed in {Duration}ms",
            operation, durationMs);
    }

    public void LogBusinessEvent(string eventName, object data, params object[] properties)
    {
        // Performance optimization: Use direct logging instead of anonymous objects
        var allProperties = PrependStandardProperties(properties);
        _logger.LogInformation("Business Event: {EventName} - {Data}",
            eventName, data);
    }

    public void LogApiRequest(string method, string url, int statusCode, TimeSpan duration, params object[] properties)
    {
        // Performance optimization: Use direct logging instead of anonymous objects
        var durationMs = (long)duration.TotalMilliseconds;
        var logLevel = statusCode >= 400 ? LogLevel.Warning : LogLevel.Information;
        var allProperties = PrependStandardProperties(properties);
        _logger.Log(logLevel, "API Request: {Method} {Url} - {StatusCode} in {Duration}ms",
            method, url, statusCode, durationMs);
    }

    public void LogWithCorrelation(string correlationId, string message, params object[] properties)
    {
        var correlationProperties = new object[] { correlationId }.Concat(properties).ToArray();
        _logger.LogInformation(message, PrependStandardProperties(correlationProperties));
    }

    public void LogBatch(params (string message, object[] properties)[] messages)
    {
        // Performance optimization: Parallel processing for true batch performance
        if (messages.Length > 100) // Use parallel for large batches
        {
            Parallel.ForEach(messages, message =>
            {
                _logger.LogInformation(message.message, PrependStandardProperties(message.properties));
            });
        }
        else // Use sequential for small batches to avoid overhead
        {
            foreach (var (message, properties) in messages)
            {
                _logger.LogInformation(message, PrependStandardProperties(properties));
            }
        }
    }

    public void LogBatchOptimized(params (string message, object[] properties)[] messages)
    {
        // Ultra-high-performance batch logging with object pooling
        if (messages.Length > 1000) // Use parallel for very large batches
        {
            Parallel.ForEach(messages, message =>
            {
                var buffer = PrependStandardProperties(message.properties);
                try
                {
                    _logger.LogInformation(message.message, buffer);
                }
                finally
                {
                    // Return buffer to pool for reuse
                    ReturnPropertyBuffer(buffer);
                }
            });
        }
        else if (messages.Length > 100) // Use parallel for large batches
        {
            Parallel.ForEach(messages, message =>
            {
                var buffer = PrependStandardProperties(message.properties);
                try
                {
                    _logger.LogInformation(message.message, buffer);
                }
                finally
                {
                    ReturnPropertyBuffer(buffer);
                }
            });
        }
        else // Use sequential for small batches
        {
            foreach (var (message, properties) in messages)
            {
                var buffer = PrependStandardProperties(properties);
                try
                {
                    _logger.LogInformation(message, buffer);
                }
                finally
                {
                    ReturnPropertyBuffer(buffer);
                }
            }
        }
    }

    public void LogException(Exception exception, string message, params object[] context)
    {
        var properties = PrependStandardProperties(context);
        _logger.LogError(exception, message, properties);
    }

    public void LogBusinessException(Exception exception, string? correlationId = null, string? userId = null, string? requestId = null)
    {
        // Performance optimization: Use pre-allocated array instead of List
        var contextCount = 0;
        if (!string.IsNullOrEmpty(correlationId)) contextCount += 2;
        if (!string.IsNullOrEmpty(userId)) contextCount += 2;
        if (!string.IsNullOrEmpty(requestId)) contextCount += 2;

        var context = new object[contextCount];
        var index = 0;

        if (!string.IsNullOrEmpty(correlationId))
        {
            context[index++] = "CorrelationId";
            context[index++] = correlationId;
        }

        if (!string.IsNullOrEmpty(userId))
        {
            context[index++] = "UserId";
            context[index++] = userId;
        }

        if (!string.IsNullOrEmpty(requestId))
        {
            context[index++] = "RequestId";
            context[index++] = requestId;
        }

        var properties = PrependStandardProperties(context);
        _logger.LogError(exception, "Business exception occurred: {ExceptionType}", exception.GetType().Name);
    }

    public void LogValidationErrors(object[] validationErrors, string? correlationId = null, string? userId = null)
    {
        // Performance optimization: Use pre-allocated array instead of List
        var contextCount = 2; // ValidationErrors + validationErrors
        if (!string.IsNullOrEmpty(correlationId)) contextCount += 2;
        if (!string.IsNullOrEmpty(userId)) contextCount += 2;

        var context = new object[contextCount];
        var index = 0;

        if (!string.IsNullOrEmpty(correlationId))
        {
            context[index++] = "CorrelationId";
            context[index++] = correlationId;
        }

        if (!string.IsNullOrEmpty(userId))
        {
            context[index++] = "UserId";
            context[index++] = userId;
        }

        context[index++] = "ValidationErrors";
        context[index++] = validationErrors;

        var properties = PrependStandardProperties(context);
        _logger.LogWarning("Validation errors occurred: {ErrorCount} errors", validationErrors.Length);
    }

    private object[] PrependStandardProperties(object[] properties)
    {
        // Performance optimization: Use object pooling for high-volume scenarios
        var totalLength = 4 + properties.Length;
        
        // Try to get a buffer from the pool
        if (!_propertyBufferPool.TryDequeue(out var result) || result.Length < totalLength)
        {
            // Create new buffer if pool is empty or buffer is too small
            result = new object[Math.Max(totalLength, MaxProperties)];
        }

        // Set standard properties
        result[0] = _serviceName;
        result[1] = _version;
        result[2] = _machineName;
        result[3] = _processId;

        // Copy properties array for better performance
        if (properties.Length > 0)
        {
            Array.Copy(properties, 0, result, 4, properties.Length);
        }

        // Return buffer to pool after use (caller should return it)
        return result;
    }

    /// <summary>
    /// Returns a property buffer to the pool for reuse
    /// </summary>
    /// <param name="buffer">The buffer to return</param>
    public static void ReturnPropertyBuffer(object[] buffer)
    {
        if (buffer != null && buffer.Length <= MaxProperties)
        {
            _propertyBufferPool.Enqueue(buffer);
        }
    }
}