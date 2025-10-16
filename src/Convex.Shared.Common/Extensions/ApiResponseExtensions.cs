using Convex.Shared.Common.Models;
using Convex.Shared.Common.Constants;

namespace Convex.Shared.Common.Extensions;

/// <summary>
/// Extension methods for creating API responses
/// </summary>
public static class ApiResponseExtensions
{
    /// <summary>
    /// Create a successful API response
    /// </summary>
    public static ApiResponse<T> Success<T>(T data, string? message = null, string? correlationId = null, string? requestId = null)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Data = data,
            Message = message,
            Metadata = new ResponseMetadata
            {
                CorrelationId = correlationId,
                RequestId = requestId,
                Version = "1.0"
            }
        };
    }

    /// <summary>
    /// Create a successful API response for created resources
    /// </summary>
    public static ApiResponse<T> Created<T>(T data, string? message = "Resource created successfully", string? correlationId = null, string? requestId = null)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Data = data,
            Message = message,
            Metadata = new ResponseMetadata
            {
                CorrelationId = correlationId,
                RequestId = requestId,
                Version = "1.0"
            }
        };
    }

    /// <summary>
    /// Create a successful API response for updated resources
    /// </summary>
    public static ApiResponse<T> Updated<T>(T data, string? message = "Resource updated successfully", string? correlationId = null, string? requestId = null)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Data = data,
            Message = message,
            Metadata = new ResponseMetadata
            {
                CorrelationId = correlationId,
                RequestId = requestId,
                Version = "1.0"
            }
        };
    }

    /// <summary>
    /// Create a successful API response for deleted resources
    /// </summary>
    public static ApiResponse<object> Deleted(string? message = "Resource deleted successfully", string? correlationId = null, string? requestId = null)
    {
        return new ApiResponse<object>
        {
            Success = true,
            Data = null,
            Message = message,
            Metadata = new ResponseMetadata
            {
                CorrelationId = correlationId,
                RequestId = requestId,
                Version = "1.0"
            }
        };
    }

    /// <summary>
    /// Create a paginated API response
    /// </summary>
    public static PaginatedResponse<T> Paginated<T>(
        IEnumerable<T> data, 
        int page, 
        int pageSize, 
        long totalCount, 
        string? message = null, 
        string? correlationId = null, 
        string? requestId = null)
    {
        var response = new PaginatedResponse<T>(data, page, pageSize, totalCount)
        {
            Message = message,
            Metadata = new ResponseMetadata
            {
                CorrelationId = correlationId,
                RequestId = requestId,
                Version = "1.0",
                Pagination = new PaginationMetadata
                {
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                    HasNext = page < (int)Math.Ceiling((double)totalCount / pageSize),
                    HasPrevious = page > 1
                }
            }
        };

        return response;
    }

    /// <summary>
    /// Create an error API response
    /// </summary>
    public static ApiResponse<T> Error<T>(string errorCode, string message, string? correlationId = null, string? requestId = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Data = default,
            Message = message,
            Metadata = new ResponseMetadata
            {
                CorrelationId = correlationId,
                RequestId = requestId,
                Version = "1.0"
            }
        };
    }

    /// <summary>
    /// Create a validation error API response
    /// </summary>
    public static ApiResponse<T> ValidationError<T>(string message, object? details = null, string? correlationId = null, string? requestId = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Data = default,
            Message = message,
            Metadata = new ResponseMetadata
            {
                CorrelationId = correlationId,
                RequestId = requestId,
                Version = "1.0"
            }
        };
    }

    /// <summary>
    /// Create a not found API response
    /// </summary>
    public static ApiResponse<T> NotFound<T>(string message = "Resource not found", string? correlationId = null, string? requestId = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Data = default,
            Message = message,
            Metadata = new ResponseMetadata
            {
                CorrelationId = correlationId,
                RequestId = requestId,
                Version = "1.0"
            }
        };
    }

    /// <summary>
    /// Create an unauthorized API response
    /// </summary>
    public static ApiResponse<T> Unauthorized<T>(string message = "Authentication required", string? correlationId = null, string? requestId = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Data = default,
            Message = message,
            Metadata = new ResponseMetadata
            {
                CorrelationId = correlationId,
                RequestId = requestId,
                Version = "1.0"
            }
        };
    }

    /// <summary>
    /// Create a forbidden API response
    /// </summary>
    public static ApiResponse<T> Forbidden<T>(string message = "Access denied", string? correlationId = null, string? requestId = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Data = default,
            Message = message,
            Metadata = new ResponseMetadata
            {
                CorrelationId = correlationId,
                RequestId = requestId,
                Version = "1.0"
            }
        };
    }

    /// <summary>
    /// Create a rate limit exceeded API response
    /// </summary>
    public static ApiResponse<T> RateLimitExceeded<T>(string message = "Rate limit exceeded", string? correlationId = null, string? requestId = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Data = default,
            Message = message,
            Metadata = new ResponseMetadata
            {
                CorrelationId = correlationId,
                RequestId = requestId,
                Version = "1.0"
            }
        };
    }

    /// <summary>
    /// Create an internal server error API response
    /// </summary>
    public static ApiResponse<T> InternalServerError<T>(string message = "Internal server error", string? correlationId = null, string? requestId = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Data = default,
            Message = message,
            Metadata = new ResponseMetadata
            {
                CorrelationId = correlationId,
                RequestId = requestId,
                Version = "1.0"
            }
        };
    }
}
