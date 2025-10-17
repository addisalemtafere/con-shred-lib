namespace Convex.Shared.Utilities.Cache;

/// <summary>
/// Extended cache utility functions
/// </summary>
public static class ExtendedCacheHelper
{
    /// <summary>
    /// Default cache key prefix
    /// </summary>
    public const string DefaultPrefix = "convex";

    /// <summary>
    /// User cache key prefix
    /// </summary>
    private const string UserCachePrefix = "user:";

    /// <summary>
    /// Session cache key prefix
    /// </summary>
    private const string SessionCachePrefix = "session:";

    /// <summary>
    /// Ticket cache key prefix
    /// </summary>
    public const string TicketPrefix = "ticket";

    /// <summary>
    /// Transaction cache key prefix
    /// </summary>
    public const string TransactionPrefix = "transaction";

    /// <summary>
    /// Configuration cache key prefix
    /// </summary>
    public const string ConfigurationPrefix = "config";

    /// <summary>
    /// Creates a cache key with prefix
    /// </summary>
    /// <param name="prefix">Key prefix</param>
    /// <param name="identifier">Key identifier</param>
    /// <returns>Cache key</returns>
    public static string CreateKey(string prefix, string identifier)
    {
        return $"{prefix}:{identifier}";
    }

    /// <summary>
    /// Creates a cache key with prefix and environment
    /// </summary>
    /// <param name="prefix">Key prefix</param>
    /// <param name="identifier">Key identifier</param>
    /// <param name="environment">Environment name</param>
    /// <returns>Cache key</returns>
    public static string CreateKey(string prefix, string identifier, string environment)
    {
        return $"{environment}:{prefix}:{identifier}";
    }

    /// <summary>
    /// Creates a ticket cache key
    /// </summary>
    /// <param name="ticketId">Ticket ID</param>
    /// <returns>Ticket cache key</returns>
    public static string CreateTicketKey(Guid ticketId)
    {
        return CreateKey(TicketPrefix, ticketId.ToString());
    }

    /// <summary>
    /// Creates a ticket cache key with environment
    /// </summary>
    /// <param name="ticketId">Ticket ID</param>
    /// <param name="environment">Environment name</param>
    /// <returns>Ticket cache key</returns>
    public static string CreateTicketKey(Guid ticketId, string environment)
    {
        return CreateKey(TicketPrefix, ticketId.ToString(), environment);
    }

    /// <summary>
    /// Creates a transaction cache key
    /// </summary>
    /// <param name="transactionId">Transaction ID</param>
    /// <returns>Transaction cache key</returns>
    public static string CreateTransactionKey(Guid transactionId)
    {
        return CreateKey(TransactionPrefix, transactionId.ToString());
    }

    /// <summary>
    /// Creates a transaction cache key with environment
    /// </summary>
    /// <param name="transactionId">Transaction ID</param>
    /// <param name="environment">Environment name</param>
    /// <returns>Transaction cache key</returns>
    public static string CreateTransactionKey(Guid transactionId, string environment)
    {
        return CreateKey(TransactionPrefix, transactionId.ToString(), environment);
    }

    /// <summary>
    /// Creates a configuration cache key
    /// </summary>
    /// <param name="configKey">Configuration key</param>
    /// <returns>Configuration cache key</returns>
    public static string CreateConfigurationKey(string configKey)
    {
        return CreateKey(ConfigurationPrefix, configKey);
    }

    /// <summary>
    /// Creates a configuration cache key with environment
    /// </summary>
    /// <param name="configKey">Configuration key</param>
    /// <param name="environment">Environment name</param>
    /// <returns>Configuration cache key</returns>
    public static string CreateConfigurationKey(string configKey, string environment)
    {
        return CreateKey(ConfigurationPrefix, configKey, environment);
    }

    /// <summary>
    /// Creates a cache key with custom prefix
    /// </summary>
    /// <param name="customPrefix">Custom prefix</param>
    /// <param name="identifier">Key identifier</param>
    /// <returns>Cache key</returns>
    public static string CreateCustomKey(string customPrefix, string identifier)
    {
        return CreateKey(customPrefix, identifier);
    }

    /// <summary>
    /// Creates a cache key with custom prefix and environment
    /// </summary>
    /// <param name="customPrefix">Custom prefix</param>
    /// <param name="identifier">Key identifier</param>
    /// <param name="environment">Environment name</param>
    /// <returns>Cache key</returns>
    public static string CreateCustomKey(string customPrefix, string identifier, string environment)
    {
        return CreateKey(customPrefix, identifier, environment);
    }

    /// <summary>
    /// Gets cache key prefix from environment variable
    /// </summary>
    /// <param name="defaultPrefix">Default prefix if environment variable not set</param>
    /// <returns>Cache key prefix</returns>
    public static string GetCacheKeyPrefix(string defaultPrefix = DefaultPrefix)
    {
        var prefix = Environment.GetEnvironmentVariable("CACHE_KEY_PREFIX");
        return string.IsNullOrWhiteSpace(prefix) ? defaultPrefix : prefix;
    }

    /// <summary>
    /// Creates a cache key with environment prefix
    /// </summary>
    /// <param name="identifier">Key identifier</param>
    /// <returns>Cache key with environment prefix</returns>
    public static string CreateKeyWithEnvironmentPrefix(string identifier)
    {
        var prefix = GetCacheKeyPrefix();
        return $"{prefix}-{identifier}";
    }

    /// <summary>
    /// Creates a user cache key using Common constants
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <returns>User cache key</returns>
    public static string CreateUserKey(Guid userId)
    {
        return $"{UserCachePrefix}{userId}";
    }

    /// <summary>
    /// Creates a session cache key using Common constants
    /// </summary>
    /// <param name="sessionId">Session ID</param>
    /// <returns>Session cache key</returns>
    public static string CreateSessionKey(string sessionId)
    {
        return $"{SessionCachePrefix}{sessionId}";
    }
}