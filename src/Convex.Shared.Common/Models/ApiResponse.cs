namespace Convex.Shared.Common.Models;

/// <summary>
/// Standard API response wrapper
/// </summary>
/// <typeparam name="T">The type of data being returned</typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// Indicates if the operation was successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// The response data
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Error message if the operation failed
    /// </summary>
    public string? Error { get; set; }

    /// <summary>
    /// Additional error details
    /// </summary>
    public Dictionary<string, string>? Errors { get; set; }

    /// <summary>
    /// Response timestamp
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Creates a successful response
    /// </summary>
    /// <param name="data">The response data</param>
    /// <returns>Successful API response</returns>
    public static ApiResponse<T> SuccessResult(T data)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Data = data
        };
    }

    /// <summary>
    /// Creates an error response
    /// </summary>
    /// <param name="error">The error message</param>
    /// <returns>Error API response</returns>
    public static ApiResponse<T> ErrorResult(string error)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Error = error
        };
    }

    /// <summary>
    /// Creates an error response with multiple errors
    /// </summary>
    /// <param name="errors">Dictionary of errors</param>
    /// <returns>Error API response</returns>
    public static ApiResponse<T> ErrorResult(Dictionary<string, string> errors)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Errors = errors
        };
    }
}
