using Convex.Shared.Common.Exceptions;
using Convex.Shared.Common.Models;

namespace Convex.Shared.Common.Extensions;

public static class ExceptionExtensions
{
    public static Result<T> ToResult<T>(this ConvexException exception)
    {
        var error = new Error(exception.ErrorCode, exception.Message)
        {
            // You can extend Error class to include these properties if needed
            // UserMessage = exception.UserMessage,
            // Details = exception.Details,
            // Suggestions = exception.Suggestions
        };

        return Result.Failure<T>(error);
    }

    public static Result ToResult(this ConvexException exception)
    {
        var error = new Error(exception.ErrorCode, exception.Message);
        return Result.Failure(error);
    }

    public static ApiResponse<T> ToApiResponse<T>(this ConvexException exception)
    {
        var result = exception.ToResult<T>();
        return ApiResponse<T>.FromResult(result);
    }
}