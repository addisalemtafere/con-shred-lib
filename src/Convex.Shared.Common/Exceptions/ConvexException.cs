using System.Runtime.Serialization;

namespace Convex.Shared.Common.Exceptions;

/// <summary>
/// Base exception class for all Convex application exceptions
/// </summary>
[Serializable]
public abstract class ConvexException : Exception
{
    public string ErrorCode { get; }
    public string UserMessage { get; }
    public object? Details { get; }
    public string[]? Suggestions { get; }
    public string? CorrelationId { get; }

    protected ConvexException(
        string errorCode,
        string message,
        string userMessage,
        object? details = null,
        string[]? suggestions = null,
        string? correlationId = null,
        Exception? innerException = null)
        : base(message, innerException)
    {
        ErrorCode = errorCode;
        UserMessage = userMessage;
        Details = details;
        Suggestions = suggestions;
        CorrelationId = correlationId;
    }

    [Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.")]
    protected ConvexException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        ErrorCode = info.GetString(nameof(ErrorCode)) ?? string.Empty;
        UserMessage = info.GetString(nameof(UserMessage)) ?? string.Empty;
        CorrelationId = info.GetString(nameof(CorrelationId));
    }

    [Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.")]
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(ErrorCode), ErrorCode);
        info.AddValue(nameof(UserMessage), UserMessage);
        info.AddValue(nameof(CorrelationId), CorrelationId);
    }
}

/// <summary>
/// Validation exception for input validation errors
/// </summary>
[Serializable]
public class ValidationException : ConvexException
{
    public ValidationError[] ValidationErrors { get; }

    public ValidationException(
        string errorCode,
        string message,
        string userMessage,
        ValidationError[] validationErrors,
        string? correlationId = null)
        : base(errorCode, message, userMessage, null, null, correlationId)
    {
        ValidationErrors = validationErrors;
    }

    [Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.")]
    protected ValidationException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        ValidationErrors = (ValidationError[])info.GetValue(nameof(ValidationErrors), typeof(ValidationError[]))!;
    }

    [Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.")]
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(ValidationErrors), ValidationErrors);
    }
}

/// <summary>
/// Business logic exception for domain-specific errors
/// </summary>
[Serializable]
public class BusinessException : ConvexException
{
    public BusinessException(
        string errorCode,
        string message,
        string userMessage,
        object? details = null,
        string[]? suggestions = null,
        string? correlationId = null,
        Exception? innerException = null)
        : base(errorCode, message, userMessage, details, suggestions, correlationId, innerException)
    {
    }

    [Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.")]
    protected BusinessException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}

/// <summary>
/// Resource not found exception
/// </summary>
[Serializable]
public class NotFoundException : ConvexException
{
    public string ResourceType { get; }
    public string ResourceId { get; }

    public NotFoundException(
        string resourceType,
        string resourceId,
        string message,
        string userMessage,
        string? correlationId = null)
        : base("NOT_FOUND", message, userMessage, new { ResourceType = resourceType, ResourceId = resourceId }, null, correlationId)
    {
        ResourceType = resourceType;
        ResourceId = resourceId;
    }

    [Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.")]
    protected NotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        ResourceType = info.GetString(nameof(ResourceType)) ?? string.Empty;
        ResourceId = info.GetString(nameof(ResourceId)) ?? string.Empty;
    }

    [Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.")]
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(ResourceType), ResourceType);
        info.AddValue(nameof(ResourceId), ResourceId);
    }
}

/// <summary>
/// Unauthorized access exception
/// </summary>
[Serializable]
public class UnauthorizedException : ConvexException
{
    public UnauthorizedException(
        string message,
        string userMessage,
        object? details = null,
        string[]? suggestions = null,
        string? correlationId = null)
        : base("AUTH_001", message, userMessage, details, suggestions, correlationId)
    {
    }

    [Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.")]
    protected UnauthorizedException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}

/// <summary>
/// Rate limit exceeded exception
/// </summary>
[Serializable]
public class RateLimitException : ConvexException
{
    public int Limit { get; }
    public int Remaining { get; }
    public DateTime ResetTime { get; }
    public int RetryAfter { get; }

    public RateLimitException(
        int limit,
        int remaining,
        DateTime resetTime,
        int retryAfter,
        string? correlationId = null)
        : base("RATE_LIMIT", "Rate limit exceeded", "Too many requests. Please try again later.", 
               new { Limit = limit, Remaining = remaining, ResetTime = resetTime, RetryAfter = retryAfter }, 
               null, correlationId)
    {
        Limit = limit;
        Remaining = remaining;
        ResetTime = resetTime;
        RetryAfter = retryAfter;
    }

    [Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.")]
    protected RateLimitException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        Limit = info.GetInt32(nameof(Limit));
        Remaining = info.GetInt32(nameof(Remaining));
        ResetTime = info.GetDateTime(nameof(ResetTime));
        RetryAfter = info.GetInt32(nameof(RetryAfter));
    }

    [Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.")]
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(Limit), Limit);
        info.AddValue(nameof(Remaining), Remaining);
        info.AddValue(nameof(ResetTime), ResetTime);
        info.AddValue(nameof(RetryAfter), RetryAfter);
    }
}

/// <summary>
/// Validation error details
/// </summary>
public class ValidationError
{
    public string Field { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public object? AttemptedValue { get; set; }
    public object? Constraint { get; set; }
}
