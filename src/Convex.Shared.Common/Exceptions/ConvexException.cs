using System.Text.Json.Serialization;

namespace Convex.Shared.Common.Exceptions;

/// <summary>
/// Base exception class for all Convex application exceptions
/// </summary>
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

    // JSON constructor for deserialization
    [JsonConstructor]
    protected ConvexException() : base()
    {
        // Required for JSON deserialization
        ErrorCode = string.Empty;
        UserMessage = string.Empty;
    }
}