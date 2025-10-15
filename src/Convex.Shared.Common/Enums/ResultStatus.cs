namespace Convex.Shared.Common.Enums;

/// <summary>
/// Standard result status enumeration
/// </summary>
public enum ResultStatus
{
    /// <summary>
    /// Operation was successful
    /// </summary>
    Success = 0,

    /// <summary>
    /// Operation failed due to validation errors
    /// </summary>
    ValidationError = 1,

    /// <summary>
    /// Operation failed due to business logic errors
    /// </summary>
    BusinessError = 2,

    /// <summary>
    /// Operation failed due to system errors
    /// </summary>
    SystemError = 3,

    /// <summary>
    /// Operation failed due to authentication errors
    /// </summary>
    AuthenticationError = 4,

    /// <summary>
    /// Operation failed due to authorization errors
    /// </summary>
    AuthorizationError = 5,

    /// <summary>
    /// Operation failed due to not found errors
    /// </summary>
    NotFound = 6,

    /// <summary>
    /// Operation failed due to timeout
    /// </summary>
    Timeout = 7
}
