using System.Text.Json.Serialization;

namespace Convex.Shared.Common.Models;

/// <summary>
/// Generic API response wrapper for all Convex APIs
/// </summary>
/// <typeparam name="T">The data type</typeparam>
public class ApiResponse<T>
{
    [JsonPropertyName("success")]
    public bool Success { get; set; } = true;

    [JsonPropertyName("data")]
    public T? Data { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonPropertyName("metadata")]
    public ResponseMetadata? Metadata { get; set; }
}

/// <summary>
/// API response metadata
/// </summary>
public class ResponseMetadata
{
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    [JsonPropertyName("requestId")]
    public string? RequestId { get; set; }

    [JsonPropertyName("version")]
    public string? Version { get; set; }

    [JsonPropertyName("correlationId")]
    public string? CorrelationId { get; set; }

    [JsonPropertyName("pagination")]
    public PaginationMetadata? Pagination { get; set; }
}

/// <summary>
/// Pagination metadata for paginated responses
/// </summary>
public class PaginationMetadata
{
    [JsonPropertyName("page")]
    public int Page { get; set; }

    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }

    [JsonPropertyName("totalPages")]
    public int TotalPages { get; set; }

    [JsonPropertyName("totalCount")]
    public long TotalCount { get; set; }

    [JsonPropertyName("hasNext")]
    public bool HasNext { get; set; }

    [JsonPropertyName("hasPrevious")]
    public bool HasPrevious { get; set; }

    [JsonPropertyName("nextPageUrl")]
    public string? NextPageUrl { get; set; }

    [JsonPropertyName("previousPageUrl")]
    public string? PreviousPageUrl { get; set; }
}

/// <summary>
/// Paginated response wrapper
/// </summary>
/// <typeparam name="T">The data type</typeparam>
public class PaginatedResponse<T> : ApiResponse<IEnumerable<T>>
{
    public PaginatedResponse()
    {
        Metadata = new ResponseMetadata();
    }

    public PaginatedResponse(IEnumerable<T> data, int page, int pageSize, long totalCount)
    {
        Success = true;
        Data = data;
        Metadata = new ResponseMetadata
        {
            Pagination = new PaginationMetadata
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                HasNext = page < (int)Math.Ceiling((double)totalCount / pageSize),
                HasPrevious = page > 1
            }
        };
    }
}