namespace Convex.Shared.Logging.Interfaces;

/// <summary>
/// Enhanced logger interface for Convex microservices
/// </summary>
public interface IConvexLogger
{
    /// <summary>
    /// Logs an information message
    /// </summary>
    /// <param name="message">The message to log</param>
    /// <param name="properties">Additional properties</param>
    void LogInformation(string message, params object[] properties);

    /// <summary>
    /// Logs a warning message
    /// </summary>
    /// <param name="message">The message to log</param>
    /// <param name="properties">Additional properties</param>
    void LogWarning(string message, params object[] properties);

    /// <summary>
    /// Logs an error message
    /// </summary>
    /// <param name="message">The message to log</param>
    /// <param name="exception">The exception to log</param>
    /// <param name="properties">Additional properties</param>
    void LogError(string message, Exception? exception = null, params object[] properties);

    /// <summary>
    /// Logs a debug message
    /// </summary>
    /// <param name="message">The message to log</param>
    /// <param name="properties">Additional properties</param>
    void LogDebug(string message, params object[] properties);

    /// <summary>
    /// Logs a trace message
    /// </summary>
    /// <param name="message">The message to log</param>
    /// <param name="properties">Additional properties</param>
    void LogTrace(string message, params object[] properties);

    /// <summary>
    /// Logs performance metrics
    /// </summary>
    /// <param name="operation">The operation name</param>
    /// <param name="duration">The operation duration</param>
    /// <param name="properties">Additional properties</param>
    void LogPerformance(string operation, TimeSpan duration, params object[] properties);

    /// <summary>
    /// Logs business events
    /// </summary>
    /// <param name="eventName">The event name</param>
    /// <param name="data">The event data</param>
    /// <param name="properties">Additional properties</param>
    void LogBusinessEvent(string eventName, object data, params object[] properties);

    /// <summary>
    /// Logs API requests
    /// </summary>
    /// <param name="method">HTTP method</param>
    /// <param name="url">Request URL</param>
    /// <param name="statusCode">Response status code</param>
    /// <param name="duration">Request duration</param>
    /// <param name="properties">Additional properties</param>
    void LogApiRequest(string method, string url, int statusCode, TimeSpan duration, params object[] properties);

    /// <summary>
    /// Logs with correlation ID
    /// </summary>
    /// <param name="correlationId">The correlation ID</param>
    /// <param name="message">The message to log</param>
    /// <param name="properties">Additional properties</param>
    void LogWithCorrelation(string correlationId, string message, params object[] properties);

    /// <summary>
    /// High-performance batch logging for billion-record scenarios
    /// </summary>
    /// <param name="messages">Batch of messages to log</param>
    void LogBatch(params (string message, object[] properties)[] messages);

    /// <summary>
    /// Ultra-high-performance batch logging with object pooling
    /// </summary>
    /// <param name="messages">Batch of messages to log</param>
    void LogBatchOptimized(params (string message, object[] properties)[] messages);

    /// <summary>
    /// Log exception with structured context
    /// </summary>
    /// <param name="exception">The exception to log</param>
    /// <param name="message">Log message</param>
    /// <param name="context">Additional context properties</param>
    void LogException(Exception exception, string message, params object[] context);

    /// <summary>
    /// Log business exception with full context
    /// </summary>
    /// <param name="exception">The business exception</param>
    /// <param name="correlationId">Request correlation ID</param>
    /// <param name="userId">User ID if available</param>
    /// <param name="requestId">Request ID</param>
    void LogBusinessException(Exception exception, string? correlationId = null, string? userId = null, string? requestId = null);

    /// <summary>
    /// Log validation errors with structured format
    /// </summary>
    /// <param name="validationErrors">Validation errors</param>
    /// <param name="correlationId">Request correlation ID</param>
    /// <param name="userId">User ID if available</param>
    void LogValidationErrors(object[] validationErrors, string? correlationId = null, string? userId = null);
}