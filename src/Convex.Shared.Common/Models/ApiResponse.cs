namespace Convex.Shared.Common.Models;

/// <summary>
/// Standard API response wrapper
/// </summary>
/// <typeparam name="T">The type of data being returned</typeparam>
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? ErrorCode { get; set; }       
    public string? ErrorMessage { get; set; }     
    public Dictionary<string, string>? Errors { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    // Convert from Result<T> to ApiResponse<T>
    public static ApiResponse<T> FromResult(Result<T> result)
    {
        if (result.IsSuccess)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Data = result.Value
            };
        }

        var response = new ApiResponse<T>
        {
            Success = false,
            ErrorCode = result.Errors.FirstOrDefault()?.Code,
            ErrorMessage = result.Errors.FirstOrDefault()?.Message
        };

        // Handle multiple errors
        if (result.Errors.Length > 1)
        {
            response.Errors = result.Errors.ToDictionary(
                e => e.Code,
                e => e.Message
            );
        }

        return response;
    }

    // Convert from Result (non-generic) to ApiResponse<object>
    public static ApiResponse<object> FromResult(Result result)
    {
        if (result.IsSuccess)
        {
            return new ApiResponse<object>
            {
                Success = true,
                Data = null
            };
        }

        var response = new ApiResponse<object>
        {
            Success = false,
            ErrorCode = result.Errors.FirstOrDefault()?.Code,
            ErrorMessage = result.Errors.FirstOrDefault()?.Message
        };

        if (result.Errors.Length > 1)
        {
            response.Errors = result.Errors.ToDictionary(
                e => e.Code,
                e => e.Message
            );
        }

        return response;
    }

    // Keep your existing factory methods for backward compatibility
    public static ApiResponse<T> SuccessResult(T data)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Data = data
        };
    }

    public static ApiResponse<T> ErrorResult(string errorMessage)
    {
        return new ApiResponse<T>
        {
            Success = false,
            ErrorMessage = errorMessage
        };
    }

    public static ApiResponse<T> ErrorResult(string errorCode, string errorMessage)
    {
        return new ApiResponse<T>
        {
            Success = false,
            ErrorCode = errorCode,
            ErrorMessage = errorMessage
        };
    }
}