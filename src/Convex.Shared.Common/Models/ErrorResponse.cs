using System.Text.Json.Serialization;

namespace Convex.Shared.Common.Models;

/// <summary>
/// Standardized error response format for all Convex APIs
/// </summary>
public class ErrorResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; } = false;

    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    [JsonPropertyName("correlationId")]
    public string? CorrelationId { get; set; }

    [JsonPropertyName("error")]
    public ErrorDetail Error { get; set; } = new();

    [JsonPropertyName("metadata")]
    public ErrorMetadata? Metadata { get; set; }
}

/// <summary>
/// Detailed error information
/// </summary>
public class ErrorDetail
{
    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("userMessage")]
    public string UserMessage { get; set; } = string.Empty;

    [JsonPropertyName("details")]
    public object? Details { get; set; }

    [JsonPropertyName("validationErrors")]
    public ValidationError[]? ValidationErrors { get; set; }

    [JsonPropertyName("suggestions")]
    public string[]? Suggestions { get; set; }

    [JsonPropertyName("actions")]
    public ErrorAction[]? Actions { get; set; }
}

/// <summary>
/// Validation error details
/// </summary>
public class ValidationError
{
    [JsonPropertyName("field")]
    public string Field { get; set; } = string.Empty;

    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("attemptedValue")]
    public object? AttemptedValue { get; set; }

    [JsonPropertyName("constraint")]
    public object? Constraint { get; set; }
}

/// <summary>
/// Error action for user guidance
/// </summary>
public class ErrorAction
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("availableAt")]
    public DateTime? AvailableAt { get; set; }
}

/// <summary>
/// Error response metadata
/// </summary>
public class ErrorMetadata
{
    [JsonPropertyName("requestId")]
    public string? RequestId { get; set; }

    [JsonPropertyName("service")]
    public string? Service { get; set; }

    [JsonPropertyName("version")]
    public string? Version { get; set; }

    [JsonPropertyName("environment")]
    public string? Environment { get; set; }
}