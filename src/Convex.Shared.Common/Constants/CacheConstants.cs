namespace Convex.Shared.Common.Constants;

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
