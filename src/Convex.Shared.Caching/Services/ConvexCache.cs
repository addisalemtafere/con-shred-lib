using Convex.Shared.Caching.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Convex.Shared.Caching.Services;

/// <summary>
/// Cache service implementation for Convex microservices
/// </summary>
public class ConvexCache : IConvexCache
{
    private readonly IDistributedCache _distributedCache;
    private readonly JsonSerializerOptions _jsonOptions;

    public ConvexCache(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        try
        {
            var value = await _distributedCache.GetStringAsync(key);
            if (string.IsNullOrEmpty(value))
                return default;

            return JsonSerializer.Deserialize<T>(value, _jsonOptions);
        }
        catch
        {
            return default;
        }
    }

    public async Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        try
        {
            var json = JsonSerializer.Serialize(value, _jsonOptions);
            var options = new DistributedCacheEntryOptions();

            if (expiration.HasValue)
            {
                options.SetAbsoluteExpiration(expiration.Value);
            }

            await _distributedCache.SetStringAsync(key, json, options);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> RemoveAsync(string key)
    {
        try
        {
            await _distributedCache.RemoveAsync(key);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> ExistsAsync(string key)
    {
        try
        {
            var value = await _distributedCache.GetStringAsync(key);
            return !string.IsNullOrEmpty(value);
        }
        catch
        {
            return false;
        }
    }

    public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null)
    {
        var cachedValue = await GetAsync<T>(key);
        if (cachedValue != null)
            return cachedValue;

        var newValue = await factory();
        await SetAsync(key, newValue, expiration);
        return newValue;
    }

    public async Task<int> RemoveManyAsync(params string[] keys)
    {
        var removedCount = 0;
        foreach (var key in keys)
        {
            if (await RemoveAsync(key))
                removedCount++;
        }
        return removedCount;
    }

    public async Task<Dictionary<string, T?>> GetManyAsync<T>(params string[] keys)
    {
        var result = new Dictionary<string, T?>();
        foreach (var key in keys)
        {
            var value = await GetAsync<T>(key);
            result[key] = value;
        }
        return result;
    }

    public async Task<int> SetManyAsync<T>(Dictionary<string, T> items, TimeSpan? expiration = null)
    {
        if (items == null)
            return 0;

        var setCount = 0;
        foreach (var item in items)
        {
            if (await SetAsync(item.Key, item.Value, expiration))
                setCount++;
        }
        return setCount;
    }
}
