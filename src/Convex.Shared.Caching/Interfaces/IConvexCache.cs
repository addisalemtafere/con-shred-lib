namespace Convex.Shared.Caching.Interfaces;

/// <summary>
/// Cache service interface for Convex microservices
/// </summary>
public interface IConvexCache
{
    /// <summary>
    /// Gets a value from the cache
    /// </summary>
    /// <typeparam name="T">The type of value to retrieve</typeparam>
    /// <param name="key">The cache key</param>
    /// <returns>The cached value or null if not found</returns>
    Task<T?> GetAsync<T>(string key);

    /// <summary>
    /// Sets a value in the cache
    /// </summary>
    /// <typeparam name="T">The type of value to store</typeparam>
    /// <param name="key">The cache key</param>
    /// <param name="value">The value to cache</param>
    /// <param name="expiration">Optional expiration time</param>
    /// <returns>True if successful</returns>
    Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiration = null);

    /// <summary>
    /// Removes a value from the cache
    /// </summary>
    /// <param name="key">The cache key</param>
    /// <returns>True if successful</returns>
    Task<bool> RemoveAsync(string key);

    /// <summary>
    /// Checks if a key exists in the cache
    /// </summary>
    /// <param name="key">The cache key</param>
    /// <returns>True if exists</returns>
    Task<bool> ExistsAsync(string key);

    /// <summary>
    /// Gets or sets a value in the cache
    /// </summary>
    /// <typeparam name="T">The type of value</typeparam>
    /// <param name="key">The cache key</param>
    /// <param name="factory">Factory function to create value if not cached</param>
    /// <param name="expiration">Optional expiration time</param>
    /// <returns>The cached or newly created value</returns>
    Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null);

    /// <summary>
    /// Removes multiple keys from the cache
    /// </summary>
    /// <param name="keys">The cache keys to remove</param>
    /// <returns>Number of keys removed</returns>
    Task<int> RemoveManyAsync(params string[] keys);

    /// <summary>
    /// Gets multiple values from the cache
    /// </summary>
    /// <typeparam name="T">The type of values to retrieve</typeparam>
    /// <param name="keys">The cache keys</param>
    /// <returns>Dictionary of key-value pairs</returns>
    Task<Dictionary<string, T?>> GetManyAsync<T>(params string[] keys);

    /// <summary>
    /// Sets multiple values in the cache
    /// </summary>
    /// <typeparam name="T">The type of values to store</typeparam>
    /// <param name="items">Dictionary of key-value pairs</param>
    /// <param name="expiration">Optional expiration time</param>
    /// <returns>Number of items cached</returns>
    Task<int> SetManyAsync<T>(Dictionary<string, T> items, TimeSpan? expiration = null);
}
