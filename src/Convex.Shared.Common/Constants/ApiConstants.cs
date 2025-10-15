namespace Convex.Shared.Common.Constants;

/// <summary>
/// Common API constants
/// </summary>
public static class ApiConstants
{
    /// <summary>
    /// Default page size for pagination
    /// </summary>
    public const int DefaultPageSize = 20;

    /// <summary>
    /// Maximum page size for pagination
    /// </summary>
    public const int MaxPageSize = 100;

    /// <summary>
    /// Default timeout in seconds
    /// </summary>
    public const int DefaultTimeoutSeconds = 30;

    /// <summary>
    /// Maximum timeout in seconds
    /// </summary>
    public const int MaxTimeoutSeconds = 300;

    /// <summary>
    /// Default retry attempts
    /// </summary>
    public const int DefaultRetryAttempts = 3;

    /// <summary>
    /// Maximum retry attempts
    /// </summary>
    public const int MaxRetryAttempts = 10;
}

/// <summary>
/// HTTP header constants
/// </summary>
public static class HeaderConstants
{
    /// <summary>
    /// Correlation ID header name
    /// </summary>
    public const string CorrelationId = "X-Correlation-ID";

    /// <summary>
    /// Request ID header name
    /// </summary>
    public const string RequestId = "X-Request-ID";

    /// <summary>
    /// API key header name
    /// </summary>
    public const string ApiKey = "X-API-Key";

    /// <summary>
    /// User ID header name
    /// </summary>
    public const string UserId = "X-User-ID";

    /// <summary>
    /// Tenant ID header name
    /// </summary>
    public const string TenantId = "X-Tenant-ID";
}

/// <summary>
/// Cache key constants
/// </summary>
public static class CacheConstants
{
    /// <summary>
    /// Default cache expiration in minutes
    /// </summary>
    public const int DefaultExpirationMinutes = 15;

    /// <summary>
    /// Short cache expiration in minutes
    /// </summary>
    public const int ShortExpirationMinutes = 5;

    /// <summary>
    /// Long cache expiration in minutes
    /// </summary>
    public const int LongExpirationMinutes = 60;

    /// <summary>
    /// User cache key prefix
    /// </summary>
    public const string UserCachePrefix = "user:";

    /// <summary>
    /// Session cache key prefix
    /// </summary>
    public const string SessionCachePrefix = "session:";
}
